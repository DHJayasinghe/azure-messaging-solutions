using EventBus.Entities.Subscriptions;

namespace EventBus.Entities.Topics
{
    [Subscription(nameof(NotificationSubscription))]
    public sealed class PropertyTopic : Topic
    {
    }
}
