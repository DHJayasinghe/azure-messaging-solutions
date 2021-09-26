using ServiceBus.Entities.Topics;
using System;

namespace ServiceBus
{
    [Topic(nameof(JobTopic))]
    public class JobQuotedEvent : ServiceBusMessage
    {
        public Guid Id { get; set; }
    }
}
