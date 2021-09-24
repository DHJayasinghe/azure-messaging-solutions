using EventBus.Entities.Topics;

namespace EventBus.Messages
{
    [Topic(nameof(PropertyTopic))]
    public sealed class PropertyRegisterEvent : EventBusMessage
    {
    }
}
