using ServiceBus.Entities.Topics;

namespace ServiceBus
{
    [Topic(nameof(JobTopic))]
    public class JobPostedEvent : ServiceBusMessage
    {
    }
}
