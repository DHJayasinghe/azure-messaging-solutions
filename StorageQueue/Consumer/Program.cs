using Azure.Storage.Queues;
using Common;
using System;
using System.Linq;

namespace Consumer
{
    // Azure Storage Queue Polling algorithm: https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-trigger?tabs=csharp
    // Use Random Exponential Back-off algorithm to minimize transaction costs
    class Program
    {
        private readonly static string QUEUE_NAME = "sample-queue";
        private readonly static Uri QUEUE_URI = new Uri(Configuration.StorageAccount + "/" + QUEUE_NAME);
        private readonly static string SAS_TOKEN = Configuration.StorageAccountSasToken;

        static void Main(string[] args)
        {
            Console.Title = "Azure Queue Storage - Consumer";

            Console.WriteLine("Consumer starting...");

            QueueClient queueClient = new QueueClient(QUEUE_URI, new Azure.AzureSasCredential(SAS_TOKEN));

            var messages = queueClient.ReceiveMessagesAsync(maxMessages: 10, visibilityTimeout: TimeSpan.FromSeconds(45)).Result.Value;
            messages.ToList().ForEach(message =>
            {
                Console.WriteLine(message.Body);
                // Delete the message
                queueClient.DeleteMessage(message.MessageId, message.PopReceipt);
            });

            Console.WriteLine("Consumer finished...");
        }
    }
}