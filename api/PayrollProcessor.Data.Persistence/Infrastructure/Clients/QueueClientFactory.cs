using System;
using System.Text;
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
        public static Task<Response<SendReceipt>> ToQueueMessage<TMessage>(this QueueClient client, TMessage entity) where TMessage : IMessage
        {
            /*
             * The Azure storage library does not base64 encode messages anymore,
             * however the Azure Functions bindings require base64 encoding
             * https://github.com/Azure/azure-sdk-for-net/issues/10242
             */
            byte[] buffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));

            return client.SendMessageAsync(Convert.ToBase64String(buffer));
        }
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
