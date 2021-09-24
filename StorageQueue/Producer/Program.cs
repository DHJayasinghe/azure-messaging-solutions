
using Azure.Storage.Queues;
using Common;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Producer
{
    class Program
    {
        private readonly static string QUEUE_NAME = "sample-queue";
        private readonly static Uri QUEUE_URI = new Uri(Configuration.StorageAccount + "/" + QUEUE_NAME);
        private readonly static string SAS_TOKEN = Configuration.StorageAccountSasToken;
        // Push & Pop style
        static async Task Main(string[] args)
        {
            Console.Title = "Azure Queue Storage - Producer";

            Console.WriteLine("Producer starting...");

            QueueClient queueClient = new QueueClient(QUEUE_URI, new Azure.AzureSasCredential(SAS_TOKEN), new QueueClientOptions
            {
                MessageEncoding = QueueMessageEncoding.Base64
            });
            queueClient.CreateIfNotExists();

            Console.WriteLine("Sending messages...");
            for (int i = 0; i < 20; i++)
            {
                await SendMessage(queueClient, new { Message = $"Message {(i + 1)}" });
            }

            Console.WriteLine("Producer ending...");
            Console.Write("Delete queue? Y-Yes / N-No (default)");
            var key = Console.ReadLine().ToUpper();
            if (key == "Y")
                queueClient.DeleteIfExists();

            Console.WriteLine("Producer ended");
        }

        public static async Task SendMessage(QueueClient client, object message)
        {
            try
            {
                await client.SendMessageAsync(JsonConvert.SerializeObject(message));
                Console.WriteLine($"{message} - SENT");
            }
            catch
            {
                Console.WriteLine($"{message} - FAILED");
            }
        }
    }
}
