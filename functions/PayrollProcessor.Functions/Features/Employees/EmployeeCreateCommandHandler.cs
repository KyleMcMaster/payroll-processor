using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;

using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Employees
{
    public interface IEmployeeCreateCommandHandler
    {
        Task<Employee> Execute(EmployeeNew newEmployee);
    }

    public class EmployeeCreateCommandHandler : IEmployeeCreateCommandHandler
    {
        private readonly CosmosClient client;

        public EmployeeCreateCommandHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<Employee> Execute(EmployeeNew newEmployee)
        {
            var entity = EmployeeEntity.Map.From(newEmployee);

            var response = await client
                .GetDatabase(Databases.PayrollProcessor.Name)
                .GetContainer(Databases.PayrollProcessor.Containers.Employees)
                .CreateItemAsync(entity);

            return EmployeeEntity.Map.ToEmployee(response.Resource);
        }
    }
}
