using EventBus.Entities.Subscriptions;

namespace EventBus.Entities.Topics
{
    [Subscription(nameof(NotificationSubscription), nameof(SupportServiceSubscription))]
    public sealed class JobTopic : Topic
    {
    }
}
