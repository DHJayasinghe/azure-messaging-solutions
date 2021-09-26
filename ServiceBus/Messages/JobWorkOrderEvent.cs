using ServiceBus.Entities.Topics;
using System;
namespace ServiceBus
{
    [Topic(nameof(JobTopic))]
    public class JobWorkOrderEvent : ServiceBusMessage
    {
        public Guid Id
        {
            get; set;
        }
    }
}
