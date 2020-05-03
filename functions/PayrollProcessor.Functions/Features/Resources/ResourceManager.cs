using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Resources
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


        public Task CreateQueue(string queueName)
            => queueClient.GetQueueReference(queueName).CreateIfNotExistsAsync();
    }
}
