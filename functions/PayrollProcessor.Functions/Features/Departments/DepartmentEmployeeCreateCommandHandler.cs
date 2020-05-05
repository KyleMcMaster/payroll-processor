using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Departments
{
    public interface IDepartmentEmployeeCreateCommandHandler
    {
        Task<DepartmentEmployee> Execute(Employee employee);
    }

    public class DepartmentEmployeeCreateCommandHandler : IDepartmentEmployeeCreateCommandHandler
    {
        private readonly CosmosClient client;

        public DepartmentEmployeeCreateCommandHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<DepartmentEmployee> Execute(Employee employee)
        {
            var entity = DepartmentEmployeeEntity.Map.CreateNewFrom(employee);

            var response = await client
                .GetDatabase(Databases.PayrollProcessor.Name)
                .GetContainer(Databases.PayrollProcessor.Containers.Departments)
                .CreateItemAsync(entity);

            return DepartmentEmployeeEntity.Map.ToDepartmentEmployee(response.Resource);
        }
    }
}
