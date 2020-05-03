using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PayrollProcessor.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Features.Resources;
using System;
using System.Linq;
using PayrollProcessor.Core.Domain.Features.Employees;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeesTrigger
    {
        [FunctionName(nameof(GetEmployees))]
        public async Task<ActionResult<Employee[]>> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees")] HttpRequest req,
            [Table(Resource.Table.Employees)] CloudTable employeeTable,
             ILogger log)
        {
            log.LogInformation($"Retrieving all employees: [{req}]");

            var data = new TableQuerier(employeeTable);

            var employees = await data.GetAllData<Employee, EmployeeEntity>(e => EmployeeEntity.Map.ToEmployee(e));

            return employees.ToArray();
        }

        [FunctionName(nameof(CreateEmployee))]
        public async Task<ActionResult<Employee>> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees")] HttpRequest req,
            [Table(Resource.Table.Employees)] CloudTable employeeTable,
            ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            var newEmployee = await Request.Parse<EmployeeNew>(req);

            var tableResult = await employeeTable.ExecuteAsync(TableOperation.Insert(EmployeeEntity.Map.From(newEmployee)));

            if (!(tableResult.Result is EmployeeEntity employeeEntity))
            {
                throw new Exception($"Could not save payroll");
            }

            return EmployeeEntity.Map.ToEmployee(employeeEntity);
        }
    }
}
