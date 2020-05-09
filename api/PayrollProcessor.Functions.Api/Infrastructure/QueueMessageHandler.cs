using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Functions.Api.Infrastructure
{
    public class QueueMessageHandler
    {
        public static TMessage FromQueueMessage<TMessage>(CloudQueueMessage message) where TMessage : IMessage =>
            JsonConvert.DeserializeObject<TMessage>(message.AsString);
    }
}
