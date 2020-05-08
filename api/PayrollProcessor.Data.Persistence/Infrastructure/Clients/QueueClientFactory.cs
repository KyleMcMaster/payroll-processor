using Ardalis.GuardClauses;
using Azure.Storage.Queues;

namespace PayrollProcessor.Data.Persistence.Infrastructure.Clients
{
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
