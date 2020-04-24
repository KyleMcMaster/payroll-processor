using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PayrollProcessor.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeesTrigger
    {
        [FunctionName("Employees_Get")]
        public async Task<IActionResult> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees")] HttpRequest req,
            [Table("employees")] CloudTable employeeTable,
             ILogger log)
        {
            log.LogInformation($"Retrieving all employees: [{req}]");

            var data = new TableQuerier(employeeTable);

            var employees = await data.GetAllData<Employee, EmployeeEntity>(e => EmployeeEntity.Map.To(e));

            return new OkObjectResult(Response.Generate(employees));
        }

        [FunctionName("Employee_Create")]
        [return: Table("employees")]
        public async Task<EmployeeEntity> CreateEmployee(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees")] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            var employee = await Request.Parse<Employee>(req);

            employee.Id = Guid.NewGuid();

            return EmployeeEntity.Map.From(employee);
        }
    }
}
