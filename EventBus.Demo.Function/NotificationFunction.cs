using EventBus.Entities.Subscriptions;
using EventBus.Entities.Topics;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EventBus.Demo.Function
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
