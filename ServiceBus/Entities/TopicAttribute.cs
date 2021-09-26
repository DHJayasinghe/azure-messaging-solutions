using System;

namespace ServiceBus
{
    public sealed class TopicAttribute : Attribute
    {
        public string Name { get; }
        public TopicAttribute(string name) => Name = name;
    }
}
