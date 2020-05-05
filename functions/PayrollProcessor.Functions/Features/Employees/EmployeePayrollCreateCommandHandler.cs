using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Employees
{
    public interface IEmployeePayrollCreateCommandHandler
    {
        Task<EmployeePayroll> Execute(Employee employee, EmployeePayrollNew newEmployeePayroll);
    }

    public class EmployeePayrollCreateCommandHandler : IEmployeePayrollCreateCommandHandler
    {
        private readonly CosmosClient client;

        public EmployeePayrollCreateCommandHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<EmployeePayroll> Execute(Employee employee, EmployeePayrollNew newEmployeePayroll)
        {
            var entity = EmployeePayrollEntity.Map.From(employee, newEmployeePayroll);

            var response = await client
                .GetDatabase(Databases.PayrollProcessor.Name)
                .GetContainer(Databases.PayrollProcessor.Containers.Employees)
                .CreateItemAsync(entity);

            return EmployeePayrollEntity.Map.ToEmployeePayroll(response.Resource);
        }
    }
}
