using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace StorageQueue.Consumer.WebJob
{
    //References: https://docs.microsoft.com/en-us/azure/app-service/webjobs-sdk-get-started
    // https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.webjobs.host.queuesoptions.batchsize?view=azure-dotnet
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Azure Queue Storage - WebJob Consumer";

            var builder = new HostBuilder();
            // Running in development mode reduces the queue polling exponential backoff
            // that can significantly delay the amount of time it takes for the runtime to find the message and invoke the function
            builder.UseEnvironment(EnvironmentName.Development);
            builder.ConfigureWebJobs(b =>
            {
                b.AddAzureStorageCoreServices();
                b.AddAzureStorage(d =>
                {
                    //d.MaxDequeueCount = 5; // retry attempt count for messages that are failing before move to Poison queue
                    //d.MaxPollingInterval = System.TimeSpan.FromSeconds(30); // longest time to await to when a queue is empty
                    //d.VisibilityTimeout = System.TimeSpan.FromSeconds(60); // For failure messages.
                    d.BatchSize = 2; // per job batch size. default 16, max: 32
                });
            });
            builder.ConfigureLogging((context, b) =>
            {
                //b.AddConsole();
            });
            var host = builder.Build();
            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}
