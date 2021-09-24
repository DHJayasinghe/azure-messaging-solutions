using Azure.Messaging.ServiceBus;
using Common;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventBus
{
    // References: https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messages-payloads
    public sealed class EventBus : IEventBus
    {
        // make sure this connection is singleton though the application
        private readonly ServiceBusClient _client;
        private readonly ICurrentRequest _currentRequest;
        public EventBus(ICurrentRequest currentRequest)
        {
            _client = new ServiceBusClient(Configuration.ServiceBusConnection);
            _currentRequest = currentRequest ?? throw new ArgumentNullException(nameof(currentRequest));
        }

        public async Task DispatchAsync<T>(T message, CancellationToken cancellationToken) where T : EventBusMessage
        {
            var queueOrTopic = message.GetQueueName() ?? message.GetTopicName();
            if (string.IsNullOrEmpty(queueOrTopic))
                throw new ArgumentException($"{nameof(queueOrTopic)} is required");

            Console.WriteLine($"Sending message to Queue/Topic: {queueOrTopic}");
            var sender = _client.CreateSender(queueOrTopic);

            var payload = new ServiceBusMessage(JsonConvert.SerializeObject(message))
            {
                CorrelationId = _currentRequest.CorrelationId.ToString(),
                Subject = message.GetType().ToString(),
                SessionId = message.MessageId.ToString(),
                ContentType = "application/json"
            };
            Console.WriteLine($"Subject/Title: {payload.Subject}");

            await sender.SendMessageAsync(payload, cancellationToken);
        }
    }

    public interface IEventBus
    {
        Task DispatchAsync<T>(T message, CancellationToken cancellationToken) where T : EventBusMessage;
    }

    /// <summary>
    /// This need be implemented using .NET Core Middleware to read "Correlation-Id" HTTP header of incoming HTTP request, 
    /// And DI to EventBus
    /// </summary>
    public interface ICurrentRequest
    {
        public Guid CorrelationId { get; set; }
    }
}
