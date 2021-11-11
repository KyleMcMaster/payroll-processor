using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Functions.Api.Infrastructure;

namespace PayrollProcessor.Functions.Api.Features.Resources
{
    public class ResourceManager
    {
        private readonly CloudQueueClient queueClient;

        public ResourceManager()
        {
            string connectionString = EnvironmentSettings
                .Get("AzureWebJobsAzureTableStorage")
                .IfNone("UseDevelopmentStorage=true");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            queueClient = storageAccount.CreateCloudQueueClient();
        }

        public Task CreateQueue(string queueName) =>
            queueClient.GetQueueReference(queueName).CreateIfNotExistsAsync();

        public Task DeleteQueue(string queueName) =>
            queueClient.GetQueueReference(queueName).DeleteIfExistsAsync();
    }
}
