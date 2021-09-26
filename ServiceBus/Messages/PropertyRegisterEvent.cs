using ServiceBus.Entities.Topics;

namespace ServiceBus.Messages
{
    [Topic(nameof(PropertyTopic))]
    public sealed class PropertyRegisterEvent : ServiceBusMessage
    {
    }
}
