using System;

namespace EventBus
{
    public sealed class QueueAttribute : Attribute
    {
        public string Name { get; }
        public QueueAttribute(string name) => Name = name;
    }
}
