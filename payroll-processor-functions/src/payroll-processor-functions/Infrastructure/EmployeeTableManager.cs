using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace PayrollProcessor.Functions.Infrastructure
{
    public class TableManager
    {
        private readonly CloudTableClient tableClient;

        public TableManager()
        {
            string connectionString = EnvironmentSettings
                .Get("AzureWebJobsAzureTableStorage")
                .IfNone("UseDevelopmentStorage=true");

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            tableClient = storageAccount.CreateCloudTableClient();
        }

        public async Task CreateTable(string tableName)
        {
            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            await table.CreateIfNotExistsAsync();
        }
    }
}
