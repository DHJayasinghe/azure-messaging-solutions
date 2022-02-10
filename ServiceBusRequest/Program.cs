using Azure.Messaging.ServiceBus;

#region "App Initiating Stuffs"
Console.Title = "Maintenance service - staring...";

Console.Write("Enter instance number: ");
string instanceName = $"Maintenance service #{Console.ReadLine()}";

Console.Title = instanceName;

Console.Write("Enter starting session number: ");
int sessionNumber = int.Parse(Console.ReadLine());

#endregion

var serviceBusClient = new ServiceBusClient(ConnectionStrings.Value);
var requestQueue = "agency-management-inquiry";
var replyQueue = "common-reply-queue";



var requests = new List<Task<string>>();
for (int i = 0; i < 100; i++)
{
    var message = $"Hello from {instanceName}: {i}";
    requests.Add(SendReceiveAsync(message, requestQueue));
}
var messages = await Task.WhenAll(requests);
Console.WriteLine("Reply count: " + messages.Count());



async Task<string> SendReceiveAsync(string message, string destination)
{
    string sessionId = (sessionNumber++).ToString();
    var sender = serviceBusClient.CreateSender(destination);

    ServiceBusSessionReceiver sessionClient = null;
    try
    {
        var payload = new ServiceBusMessage(message)
        {
            MessageId = Guid.NewGuid().ToString(),
            SessionId = sessionId,
            ReplyTo = replyQueue,
            ReplyToSessionId = sessionId,
            TimeToLive = TimeSpan.FromMinutes(30),
            ContentType = "application/json",
        };
        Console.WriteLine($"Sending message: {message} with SessionId: {sessionId}");
        await sender.SendMessageAsync(payload);

        //Console.WriteLine($"Awaiting for a reply....");

        sessionClient = await serviceBusClient.AcceptSessionAsync(replyQueue, sessionId);
        var reply = await sessionClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));
        Console.WriteLine($"{message}: REPLY - {reply.First().Body}");

        return reply.First().Body.ToString();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"{message} : NO RESPOND");
        return "NO RESPOND";
    }
    finally
    {
        if (sessionClient is not null)
            await sessionClient.DisposeAsync();
    }
}
