using ServiceBus.Entities.Queues;

namespace ServiceBus
{
    [Queue(nameof(TimelineQueue))]
    public sealed class TimeLineMessage : ServiceBusMessage
    {
    }
}
