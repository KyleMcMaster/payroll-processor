using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;

namespace PayrollProcessor.Data.Persistence.Infrastructure.Clients
{
    public interface IMessage
    {
        string EventName { get; set; }
    }

    public class DefaultMessage
    {
        public string EventName { get; set; } = "";
    }

    public static class QueueMessageBuilder
    {
        public static Task<Response<SendReceipt>> ToQueueMessage<TMessage>(this QueueClient client, TMessage entity) where TMessage : IMessage =>
            client.SendMessageAsync(JsonConvert.SerializeObject(entity));
    }

    public interface IQueueClientFactory
    {
        QueueClient Create(string queueName);
    }

    public class QueueClientFactory : IQueueClientFactory
    {
        private readonly string connectionString;

        public QueueClientFactory(string connectionString)
        {
            Guard.Against.NullOrWhiteSpace(connectionString, nameof(connectionString));

            this.connectionString = connectionString;
        }

        public QueueClient Create(string queueName) => new QueueClient(connectionString, queueName);
    }
}
