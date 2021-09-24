using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;

namespace StorageQueue.Consumer.WebJob
{
    public class MessageHandlerFunction
    {
        public static void ProcessQueueMessage([QueueTrigger("sample-queue")] string message, ILogger logger)
        {
            //logger.LogInformation(message);
            Console.WriteLine(message);
        }
    }
}
