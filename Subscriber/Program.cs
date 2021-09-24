using Azure.Messaging.ServiceBus;
using Common;
using System;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        private static readonly ServiceBusClient BUS_CLIENT = new ServiceBusClient(Configuration.ServiceBusConnection);
        private static ServiceBusReceiver RECEIVER;
        private static readonly string TOPIC = "property_job";

        private const string NoFilterSubscriptionName = "NoFilterSubscription";
        private const string SqlFilterOnlySubscriptionName = "RedSqlFilterSubscription";
        private const string SqlFilterWithActionSubscriptionName = "BlueSqlFilterWithActionSubscription";
        private const string CorrelationFilterSubscriptionName = "ImportantCorrelationFilterSubscription";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }


        private static void PrintReceivedMessage(ServiceBusReceivedMessage message)
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), message.Subject);
            Console.WriteLine($"Received message with color: {message.ApplicationProperties["Color"]}, CorrelationId: {message.CorrelationId}");
            Console.ResetColor();
        }

        private static async Task ReceiveMessagesAsync(string subscriptionName)
        {
            await using ServiceBusReceiver subscriptionReceiver = BUS_CLIENT.CreateReceiver(
                TOPIC,
                subscriptionName,
                new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

            Console.WriteLine($"==========================================================================");
            Console.WriteLine($"{DateTime.Now} :: Receiving Messages From Subscription: {subscriptionName}");
            int receivedMessageCount = 0;
            while (true)
            {
                var receivedMessage = await subscriptionReceiver.ReceiveMessageAsync(TimeSpan.FromSeconds(1));
                if (receivedMessage != null)
                {
                    PrintReceivedMessage(receivedMessage);
                    receivedMessageCount++;
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine($"{DateTime.Now} :: Received '{receivedMessageCount}' Messages From Subscription: {subscriptionName}");
            Console.WriteLine($"==========================================================================");
            await subscriptionReceiver.CloseAsync();
        }
    }
}
