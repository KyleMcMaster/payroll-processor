using System.Linq;

using static PayrollProcessor.Data.Persistence.Infrastructure.Clients.AppResources.CosmosDb;

namespace Microsoft.Azure.Cosmos
{
    public static class CosmosClientExtensions
    {
        public static Container GetEmployeesContainer(this CosmosClient client) =>
            client.GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Employees);

        public static IOrderedQueryable<TRecord> EmployeesQueryable<TRecord>(this CosmosClient client) =>
            client.GetEmployeesContainer().GetItemLinqQueryable<TRecord>();

        public static IOrderedQueryable<TRecord> EmployeesQueryable<TRecord>(this CosmosClient client, string partitionKey) =>
            client.GetEmployeesContainer().GetItemLinqQueryable<TRecord>(requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey.ToLowerInvariant())
            });

        public static Container GetDepartmentsContainer(this CosmosClient client) =>
            client.GetContainer(Databases.PayrollProcessor.Name, Databases.PayrollProcessor.Containers.Departments);

        public static IOrderedQueryable<TRecord> DepartmentQueryable<TRecord>(this CosmosClient client) =>
            client.GetDepartmentsContainer().GetItemLinqQueryable<TRecord>();

        public static IOrderedQueryable<TRecord> DepartmentQueryable<TRecord>(this CosmosClient client, string partitionKey) =>
            client.GetDepartmentsContainer().GetItemLinqQueryable<TRecord>(requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey.ToLowerInvariant())
            });
    }

    public static class CosmosResponse
    {
        public static T Unwrap<T>(Response<T> response) => response.Resource;
    }
}
