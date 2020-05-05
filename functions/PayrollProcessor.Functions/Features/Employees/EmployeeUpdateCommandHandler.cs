using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;

using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Employees
{
    public interface IEmployeeUpdateCommandHandler
    {
        Task<Employee> Execute(Employee employee);
    }

    public class EmployeeUpdateCommandHandler : IEmployeeUpdateCommandHandler
    {
        private readonly CosmosClient client;

        public EmployeeUpdateCommandHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<Employee> Execute(Employee employee)
        {
            var entity = EmployeeEntity.Map.From(employee);

            var response = await client
                .GetDatabase(Databases.PayrollProcessor.Name)
                .GetContainer(Databases.PayrollProcessor.Containers.Employees)
                .ReplaceItemAsync(entity, entity.PartitionKey);

            return EmployeeEntity.Map.ToEmployee(response.Resource);
        }
    }
}
