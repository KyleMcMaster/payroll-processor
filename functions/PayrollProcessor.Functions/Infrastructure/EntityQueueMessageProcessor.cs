using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace PayrollProcessor.Functions.Infrastructure
{
    public interface IMessage
    {
        string Source { get; set; }
    }

    public static class QueueMessageFactory
    {
        public static CloudQueueMessage ToQueueMessage<TMessage>(TMessage entity) where TMessage : IMessage =>
            new CloudQueueMessage(JsonConvert.SerializeObject(entity, DefaultJsonSerializerSettings.JsonSerializerSettings));

        public static TMessage FromQueueMessage<TMessage>(CloudQueueMessage message) where TMessage : IMessage =>
            JsonConvert.DeserializeObject<TMessage>(message.AsString);
    }
}
