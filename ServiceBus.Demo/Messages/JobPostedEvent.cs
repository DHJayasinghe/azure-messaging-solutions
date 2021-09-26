using ServiceBus.Entities.Topics;
using System;

namespace ServiceBus.Demo
{
    [Topic(nameof(JobTopic))]
    public partial class JobPostedEvent : ServiceBusMessage
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public JobPostedEvent() => MessageId = Guid.NewGuid();
        public JobPostedEvent(Guid id) : this()
        {
            MessageId = id;
        }
    }
}
