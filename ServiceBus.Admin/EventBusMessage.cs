using EventBus.Entities.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EventBus
{
    public abstract class EventBusMessage
    {
        /// <summary>
        /// Use for message duplicate detection -> This is mostly an Aggregate Id
        /// </summary>
        public Guid MessageId { get; protected set; }

        #region Helper methods

        /// <summary>
        /// Returns inherited classes
        /// </summary>
        public static IEnumerable<EventBusMessage> GetMessages() => ReflectiveEnumerator.GetEnumerableOfType<EventBusMessage>().ToList();

        /// <summary>
        /// Returns Queue attribute name
        /// </summary>
        public string GetQueueName() => GetType().GetAttributeValue((QueueAttribute queue) => queue.Name);

        /// <summary>
        /// Returns Topic attribute name
        /// </summary>
        public string GetTopicName() => GetType().GetAttributeValue((TopicAttribute topic) => topic.Name);

        /// <summary>
        /// Returns Subscriptions related to a Topic
        /// </summary>
        public IEnumerable<string> GetSubscriptions(string topicName) =>
                ReflectiveEnumerator.GetEnumerableOfType<Topic>()
                    .FirstOrDefault(d => d.GetType().Name == topicName)
                    ?.GetType().GetAttributeValue((SubscriptionAttribute subs) => subs.Names);

        #endregion
    }

    internal static class AttributeExtensions
    {
        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector
        ) where TAttribute : Attribute => type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att
                ? valueSelector(att)
                : default;
    }

    internal static class ReflectiveEnumerator
    {
        static ReflectiveEnumerator() { }

        public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class
        {
            List<T> objects = new List<T>();
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add((T)Activator.CreateInstance(type, constructorArgs));
            }
            //objects.Sort();
            return objects;
        }
    }
}
