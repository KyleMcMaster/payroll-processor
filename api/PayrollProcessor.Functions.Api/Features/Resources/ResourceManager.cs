using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Api.Infrastructure;

namespace PayrollProcessor.Functions.Api.Features.Resources
{
    public class ResourceManager
    {
        private readonly CloudTableClient tableClient;
        private readonly CloudQueueClient queueClient;

        public ResourceManager()
        {
            string connectionString = EnvironmentSettings
                .Get("AzureWebJobsAzureTableStorage")
                .IfNone("UseDevelopmentStorage=true");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            tableClient = storageAccount.CreateCloudTableClient();
            queueClient = storageAccount.CreateCloudQueueClient();
        }

        public Task CreateTable(string tableName) =>
            tableClient.GetTableReference(tableName).CreateIfNotExistsAsync();


        public Task CreateQueue(string queueName) =>
            queueClient.GetQueueReference(queueName).CreateIfNotExistsAsync();

        public Task DeleteTable(string tableName) =>
            tableClient.GetTableReference(tableName).DeleteIfExistsAsync();

        public Task DeleteQueue(string queueName) =>
            queueClient.GetQueueReference(queueName).DeleteIfExistsAsync();
    }
}
