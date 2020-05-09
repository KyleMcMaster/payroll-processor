using System.Linq;
using PayrollProcessor.Data.Persistence.Features.Departments;
using PayrollProcessor.Data.Persistence.Features.Employees;

namespace Microsoft.Azure.Cosmos
{
    public static class CosmosClientExtensions
    {
        public static Container GetEmployeesContainer(this CosmosClient client) =>
            client.GetContainer("PayrollProcessor", "Employees");

        public static IOrderedQueryable<TRecord> EmployeesQueryable<TRecord>(this CosmosClient client) =>
            client.GetEmployeesContainer().GetItemLinqQueryable<TRecord>();

        public static IOrderedQueryable<TRecord> EmployeesQueryable<TRecord>(this CosmosClient client, string partitionKey) =>
            client.GetEmployeesContainer().GetItemLinqQueryable<TRecord>(requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            });

        public static Container GetDepartmentsContainer(this CosmosClient client) =>
            client.GetContainer("PayrollProcessor", "Departments");

        public static IOrderedQueryable<TRecord> DepartmentQueryable<TRecord>(this CosmosClient client) =>
            client.GetDepartmentsContainer().GetItemLinqQueryable<TRecord>();

        public static IOrderedQueryable<TRecord> DepartmentQueryable<TRecord>(this CosmosClient client, string partitionKey) =>
            client.GetDepartmentsContainer().GetItemLinqQueryable<TRecord>(requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(partitionKey)
            });
    }
}
