using System;

namespace ServiceBus
{
    public sealed class QueueAttribute : Attribute
    {
        public string Name { get; }
        public QueueAttribute(string name) => Name = name;
    }
}
