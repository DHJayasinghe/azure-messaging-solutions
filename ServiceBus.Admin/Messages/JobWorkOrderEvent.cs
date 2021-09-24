using EventBus.Entities.Topics;
using System;
namespace EventBus
{
    [Topic(nameof(JobTopic))]
    public class JobWorkOrderEvent : EventBusMessage
    {
        public Guid Id
        {
            get; set;
        }
    }
}
