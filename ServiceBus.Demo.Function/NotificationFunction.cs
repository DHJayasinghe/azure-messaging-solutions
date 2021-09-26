using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using ServiceBus.Entities.Subscriptions;
using ServiceBus.Entities.Topics;

namespace ServiceBus.Demo.Function
{
    public static class NotificationFunction
    {
        [FunctionName("NotificationFunction")]
        public static void Run([ServiceBusTrigger(nameof(JobTopic), nameof(NotificationSubscription), Connection = "AzureWebJobsStorage")] string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus {nameof(JobTopic)} trigger for {nameof(NotificationSubscription)} function processed message: {mySbMsg}");
        }
    }
}
