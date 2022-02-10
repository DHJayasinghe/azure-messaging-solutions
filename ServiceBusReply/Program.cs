using Azure.Messaging.ServiceBus;

#region "App Initiating Stuffs"
Console.Title = "Agency Mgmt service - staring...";

Console.Write("Enter instance number: ");
string instanceName = $"Agency Mgmt service #{Console.ReadLine()}";

Console.Title = instanceName;

#endregion

var client = new ServiceBusClient(ConnectionStrings.Value);
var inquiriesForMeQueue = "agency-management-inquiry";

var requestProcessor = client.CreateSessionProcessor(inquiriesForMeQueue, new ServiceBusSessionProcessorOptions()
{
    PrefetchCount = 10,
    AutoCompleteMessages = true,
});
try
{
    requestProcessor.ProcessMessageAsync += MessageHandler;   // handler to process messages
    requestProcessor.ProcessErrorAsync += ErrorHandler;  // handler to process any errors
    await requestProcessor.StartProcessingAsync();

    //Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("Stopping the receiver...");
    await requestProcessor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    await requestProcessor.DisposeAsync();
    await client.DisposeAsync();
}



async Task MessageHandler(ProcessSessionMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received message: {body} for session: {args.SessionId}");

    await SendReply($"Hello back - {instanceName}:{args.SessionId}", args.Message);
}
static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}

async Task SendReply(string replyMessage, ServiceBusReceivedMessage receivedMessage)
{
    var replier = client.CreateSender(receivedMessage.ReplyTo);
    var payload = new ServiceBusMessage(replyMessage)
    {
        MessageId = Guid.NewGuid().ToString(),
        SessionId = receivedMessage.ReplyToSessionId,
        ContentType = "application/json",
        TimeToLive = TimeSpan.FromMinutes(30),
    };
    Console.WriteLine($"Sending reply: {replyMessage} for session Id: {receivedMessage.ReplyToSessionId} to queue: {receivedMessage.ReplyTo}");
    await replier.SendMessageAsync(payload);
}
