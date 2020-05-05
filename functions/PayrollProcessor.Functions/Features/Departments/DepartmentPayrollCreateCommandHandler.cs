using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using static PayrollProcessor.Functions.Infrastructure.AppResources.CosmosDb;

namespace PayrollProcessor.Functions.Features.Departments
{
    public interface IDepartmentPayrollCreateCommandHandler
    {
        Task<DepartmentPayroll> Execute(Employee employee, EmployeePayroll payroll);
    }

    public class DepartmentPayrollCreateCommandHandler : IDepartmentPayrollCreateCommandHandler
    {
        private readonly CosmosClient client;

        public DepartmentPayrollCreateCommandHandler(CosmosClient client) =>
            this.client = client ?? throw new ArgumentNullException(nameof(client));

        public async Task<DepartmentPayroll> Execute(Employee employee, EmployeePayroll employeePayroll)
        {
            var entity = DepartmentPayrollEntity.Map.CreateNewFrom(employee, employeePayroll);

            var response = await client
                .GetDatabase(Databases.PayrollProcessor.Name)
                .GetContainer(Databases.PayrollProcessor.Containers.Departments)
                .CreateItemAsync(entity);

            return DepartmentPayrollEntity.Map.ToDepartmentPayroll(response.Resource);
        }
    }
}
