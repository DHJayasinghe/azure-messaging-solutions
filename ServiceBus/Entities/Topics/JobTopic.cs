using ServiceBus.Entities.Subscriptions;

namespace ServiceBus.Entities.Topics
{
    [Subscription(nameof(NotificationSubscription), nameof(SupportServiceSubscription))]
    public sealed class JobTopic : Topic
    {
    }
}
