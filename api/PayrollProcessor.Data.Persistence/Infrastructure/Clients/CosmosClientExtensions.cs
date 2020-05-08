using System.Linq;
using PayrollProcessor.Data.Persistence.Features.Employees;

namespace Microsoft.Azure.Cosmos
{
    public static class CosmosClientExtensions
    {
        public static Container GetEmployeesContainer(this CosmosClient client) =>
            client.GetContainer("PayrollProcessor", "Employees");

        public static IOrderedQueryable<EmployeeRecord> EmployeesQueryable(this CosmosClient client) =>
            client.GetEmployeesContainer().GetItemLinqQueryable<EmployeeRecord>();
    }
}
