using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PayrollProcessor.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage.Table;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeesTrigger
    {
        public const string CONNECTION = "AzureTableStorage";

        [FunctionName("Employees_Get")]
        public async Task<IActionResult> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees")] HttpRequest req,
            [Table("employees", Connection = CONNECTION)] CloudTable employeeTable,
             ILogger log)
        {
            log.LogInformation($"Retrieving all employees: [{req}]");

            var data = new TableQuerier(employeeTable);

            var employees = await data.GetAllData<Employee, EmployeeEntity>(e => EmployeeEntity.Map.To(e));

            return new OkObjectResult(Response.Generate(employees));
        }

        [FunctionName("Employee_Create")]
        [return: Table("employees", Connection = CONNECTION)]
        public async Task<EmployeeEntity> CreateEmployee(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees")] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            var employee = await Request.Parse<Employee>(req);

            return EmployeeEntity.Map.From(employee);
        }

        [FunctionName("EmployeesTable_Create")]
        public async Task<IActionResult> CreateEmployeesTable(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees/table")] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"Creating employees table: [{req}]");

            var tableManager = new TableManager();

            await tableManager.CreateTable("employees");

            return new OkResult();
        }
    }
}
