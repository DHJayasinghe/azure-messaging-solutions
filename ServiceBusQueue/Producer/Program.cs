using Azure.Messaging.ServiceBus;
using Common;
using System;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        // connection string to your Service Bus namespace
        static readonly string CONNECTION_STRING = Configuration.ServiceBusConnection;
        static readonly string QUEUE_NAME = "sample-queue";

        // the client that owns the connection and can be used to create senders and receivers
        static ServiceBusClient client;
        static ServiceBusSender sender;

        static async Task Main()
        {
            Console.Title = "Azure Service Bus Queue - Producer";

            client = new ServiceBusClient(CONNECTION_STRING);
            sender = client.CreateSender(QUEUE_NAME);

            int choice = 1;
            while (choice == 1 || choice == 2)
            {
                choice = AskToSelectAChoice();

                if (choice == 1)
                    await PublishMessageBatch();
                else if (choice == 2)
                    await PublishSingleMessages();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();

            try
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
                Console.WriteLine("Finished");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static int AskToSelectAChoice()
        {
            Console.WriteLine();
            Console.WriteLine("Select your choice of queue transmission");
            Console.WriteLine("[1] - Batch messages");
            Console.WriteLine("[2] - Single messages");
            Console.Write("Your choice [1]/[2] : ");

            if (!int.TryParse(Console.ReadLine(), out int choice))
                return 0;

            return choice;
        }

        private static async Task PublishMessageBatch()
        {
            Console.Write("Number of messages for the batch: ");
            int numOfMessages = Convert.ToInt32(Console.ReadLine());

            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            for (int i = 1; i <= numOfMessages; i++)
            {
                // try adding a message to the batch
                if (!messageBatch.TryAddMessage(new ServiceBusMessage($"Batch Message {i}")))
                {
                    // if it is too large for the batch
                    throw new Exception($"The message {i} is too large to fit in the batch.");
                }
            }

            // Use the producer client to send the batch of messages to the Service Bus queue
            await sender.SendMessagesAsync(messageBatch);
            Console.WriteLine($"A batch of {numOfMessages} messages has been published to the queue.");
        }

        private static async Task PublishSingleMessages()
        {
            Console.Write("Number of messages to sent: ");
            int numOfMessages = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < numOfMessages; i++)
            {
                string message = $"Single Message: {(i + 1)}";
                Console.WriteLine($"Sending message: {message}");
                await sender.SendMessageAsync(new ServiceBusMessage($"Single Message: {message}"));
            }
        }
    }
}
