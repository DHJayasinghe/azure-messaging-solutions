using EventBus.Entities.Topics;

namespace EventBus
{
    [Topic(nameof(JobTopic))]
    public class JobPostedEvent : EventBusMessage
    {
    }
}
