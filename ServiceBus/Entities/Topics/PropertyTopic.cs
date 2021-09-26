using ServiceBus.Entities.Subscriptions;

namespace ServiceBus.Entities.Topics
{
    [Subscription(nameof(NotificationSubscription))]
    public sealed class PropertyTopic : Topic
    {
    }
}
