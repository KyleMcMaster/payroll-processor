using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Payroll.Processor.Functions.Infrastructure
{
    public static class EntityQueueMessageProcessor
    {
        public static CloudQueueMessage ToQueueMessage<T>(T entity) where T : ITableEntity
        {
            return new CloudQueueMessage(JsonConvert.SerializeObject(entity, DefaultJsonSerializerSettings.JsonSerializerSettings));
        }

        public static T FromQueueMessage<T>(CloudQueueMessage message) where T : ITableEntity
        {
            return JsonConvert.DeserializeObject<T>(message.AsString);
        }
    }
}
