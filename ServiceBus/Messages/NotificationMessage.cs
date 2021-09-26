using ServiceBus.Entities.Queues;

namespace ServiceBus
{
    [Queue(nameof(NotificationQueue))]
    public sealed class NotificationMessage : ServiceBusMessage
    {
    }
}
