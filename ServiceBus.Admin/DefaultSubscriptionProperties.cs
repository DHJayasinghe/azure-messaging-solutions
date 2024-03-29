﻿using System;

namespace ServiceBus.Admin
{
    internal class DefaultSubscriptionProperties
    {
        public bool RequiresSession { get; internal set; }
        public TimeSpan LockDuration { get; internal set; }
        public int MaxDeliveryCount { get; internal set; }
        public TimeSpan DefaultMessageTimeToLive { get; internal set; }
    }
}