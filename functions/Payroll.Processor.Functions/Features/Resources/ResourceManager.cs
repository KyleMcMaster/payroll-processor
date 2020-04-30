using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using Payroll.Processor.Functions.Infrastructure;

namespace Payroll.Processor.Functions.Features.Resources
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

        public async Task CreateTable(string tableName)
        {
            await tableClient.GetTableReference(tableName).CreateIfNotExistsAsync();
        }

        public async Task CreateQueue(string queueName)
        {
            await queueClient.GetQueueReference(queueName).CreateIfNotExistsAsync();
        }
    }
}
