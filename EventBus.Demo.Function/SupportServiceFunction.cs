using EventBus.Entities.Subscriptions;
using EventBus.Entities.Topics;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace EventBus.Demo.Function
{
    public static class SupportServiceFunction
    {
        [FunctionName("SupportServiceFunction")]
        public static void Run([ServiceBusTrigger(nameof(JobTopic), nameof(SupportServiceSubscription), Connection = "AzureWebJobsStorage")] string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus {nameof(JobTopic)} trigger for {nameof(SupportServiceSubscription)} function processed message: {mySbMsg}");
        }
    }
}
