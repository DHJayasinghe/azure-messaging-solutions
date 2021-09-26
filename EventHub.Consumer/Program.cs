using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Common;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EventHub.Consumer
{
    class Program
    {
        const string consumerGroup =
            EventHubConsumerClient.DefaultConsumerGroupName;

        static async Task Main(string[] args)
        {
            //await GetPartitionInfo();

            var readZero = ReadFromPartition("0");
            var readOne = ReadFromPartition("1");
            var readTwo = ReadFromPartition("2");
            var readThree = ReadFromPartition("3");

            await Task.WhenAll(readZero, readOne, readTwo, readThree);
        }

        static async Task ReadFromPartition(string partitionNumber)
        {
            var cancelToken = new CancellationTokenSource();
            cancelToken.CancelAfter(TimeSpan.FromSeconds(120));

            await using (var consumerClient =
                new EventHubConsumerClient(consumerGroup, Configuration.EventHubConnection, Configuration.EventHubName))
            {
                try
                {
                    PartitionProperties props =
                        await consumerClient
                            .GetPartitionPropertiesAsync(partitionNumber);

                    EventPosition startingPosition =
                        EventPosition.FromSequenceNumber(
                             //props.LastEnqueuedSequenceNumber
                             props.BeginningSequenceNumber
                        );

                    await foreach (PartitionEvent partitionEvent in consumerClient
                        .ReadEventsFromPartitionAsync(partitionNumber, startingPosition, cancelToken.Token))
                    {
                        Console.WriteLine("***** NEW COFFEE *****");

                        string partitionId = partitionEvent.Partition.PartitionId;
                        var sequenceNumber = partitionEvent.Data.SequenceNumber;
                        var key = partitionEvent.Data.PartitionKey;

                        Console.WriteLine($"Partition Id: {partitionId}{Environment.NewLine}Sequence Number: {sequenceNumber}{Environment.NewLine}Partition Key: {key}");

                        var coffee = JsonSerializer
                            .Deserialize<CoffeeData>(partitionEvent.Data.Body.Span);

                        Console.WriteLine($"Type: {coffee.CoffeeType}{Environment.NewLine}Temp: {coffee.WaterTemperature}{Environment.NewLine}Date: {coffee.ReadingTime.ToShortDateString()}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    await consumerClient.CloseAsync();
                }
            }
        }

        static async Task GetPartitionInfo()
        {
            await using var consumerClient =
                new EventHubConsumerClient(consumerGroup, Configuration.EventHubConnection, Configuration.EventHubName);

            var partitionIds = await consumerClient.GetPartitionIdsAsync();

            foreach (var id in partitionIds)
            {
                var partitionInfo = await
                    consumerClient.GetPartitionPropertiesAsync(id);

                Console.WriteLine("***** NEW PARTITION INFO *****");
                Console.WriteLine($"Partition Id: {partitionInfo.Id}{Environment.NewLine}Empty? {partitionInfo.IsEmpty}{Environment.NewLine}Last Sequence: {partitionInfo.LastEnqueuedSequenceNumber}");
            }
        }
    }

    public class CoffeeData
    {
        public static readonly string[] AllCoffeeTypes = { "Sumatra", "Columbian", "French" };

        public double WaterTemperature { get; set; }
        public DateTime ReadingTime { get; set; }
        public string CoffeeType { get; set; }
    }
}
