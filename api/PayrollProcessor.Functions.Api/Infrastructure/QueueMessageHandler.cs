using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Functions.Api.Infrastructure;

public class QueueMessageHandler
{
    public static TMessage FromQueueMessage<TMessage>(CloudQueueMessage queueMessage) where TMessage : IMessage =>
#pragma warning disable CS8603 // Possible null reference return.
            JsonConvert.DeserializeObject<TMessage>(queueMessage.AsString);
#pragma warning restore CS8603 // Possible null reference return.

    public static string GetEventName(CloudQueueMessage queueMessage) =>
        JsonConvert.DeserializeObject<DefaultMessage>(queueMessage.AsString)?.EventName ?? "";
}
