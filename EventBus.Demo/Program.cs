using System;
using System.Threading;
using System.Threading.Tasks;

namespace EventBus.Demo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var sampleCurrentRequest = new SampleCurrentRequest();
            Console.WriteLine($"Correlation Id: {sampleCurrentRequest.CorrelationId}");

            var eventBus = new EventBus(currentRequest: sampleCurrentRequest);
            var cancellationToken = new CancellationToken();
            try
            {
                await eventBus.DispatchAsync(new JobPostedEvent
                {
                    Title = "My First Job",
                    Description = "This is my description",
                    DateTimeCreated = DateTime.Now
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
            }
        }
    }

    public sealed class SampleCurrentRequest : ICurrentRequest
    {
        public Guid CorrelationId { get; set; } = Guid.NewGuid();
    }
}
