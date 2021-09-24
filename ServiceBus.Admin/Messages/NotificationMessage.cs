using EventBus.Entities.Queues;

namespace EventBus
{
    [Queue(nameof(NotificationQueue))]
    public sealed class NotificationMessage : EventBusMessage
    {
    }
}
