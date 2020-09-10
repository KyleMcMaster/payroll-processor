using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Functions.Api.Infrastructure
{
    public class QueueMessageHandler
    {
        public static TMessage FromQueueMessage<TMessage>(CloudQueueMessage queueMessage) where TMessage : IMessage =>
            JsonConvert.DeserializeObject<TMessage>(queueMessage.AsString);

        public static string GetEventName(CloudQueueMessage queueMessage) =>
            JsonConvert.DeserializeObject<DefaultMessage>(queueMessage.AsString)?.EventName ?? "";
    }
}
