using EventBus.Entities.Queues;

namespace EventBus
{
    [Queue(nameof(TimelineQueue))]
    public sealed class TimeLineMessage : EventBusMessage
    {
    }
}
