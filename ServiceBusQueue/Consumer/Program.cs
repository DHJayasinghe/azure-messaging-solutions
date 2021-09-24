using Azure.Messaging.ServiceBus;
using Common;
using System;
using System.Threading.Tasks;

namespace Consumer
{
    class Program
    {
        // connection string to your Service Bus namespace
        private static readonly string CONNECTION_STRING = Configuration.ServiceBusConnection;
        private static readonly string QUEUE_NAME = "sample-queue";

        // Client should be singleton, and safe to re-use during the lifetime of the application
        // the client that owns the connection and can be used to create senders and receivers.
        private static readonly ServiceBusClient client = new ServiceBusClient(CONNECTION_STRING);
        // the processor that reads and processes messages from the queue
        // create a processor that we can use to process the messages
        private static readonly ServiceBusProcessor processor = client.CreateProcessor(QUEUE_NAME, new ServiceBusProcessorOptions()
        {
            ReceiveMode = ServiceBusReceiveMode.PeekLock,
            PrefetchCount = 5,
            AutoCompleteMessages = true, // valid only for ServiceBusReceiveMode.PeekLock
            //MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(15),
        });

        static async Task Main()
        {
            Console.Title = "Azure Service Bus Queue - Consumer";
            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;
                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;
                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await processor.DisposeAsync();
                await client.DisposeAsync();
            }
        }

        // handle received messages
        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            //// manually complete the message. messages is deleted from the queue. 
            //// set ReceiveMode=ServiceBusReceiveMode.PeekLock and AutoCompleteMessages=false to make this work.
            //await args.CompleteMessageAsync(args.Message);
            await Task.CompletedTask;
        }

        // handle any errors when receiving messages
        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
