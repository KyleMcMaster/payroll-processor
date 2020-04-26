using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using PayrollProcessor.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Features.Resources;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeesTrigger
    {
        [FunctionName(nameof(GetEmployees))]
        public async Task<IActionResult> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees")] HttpRequest req,
            [Table(Resource.Table.Employees)] CloudTable employeeTable,
             ILogger log)
        {
            log.LogInformation($"Retrieving all employees: [{req}]");

            var data = new TableQuerier(employeeTable);

            var employees = await data.GetAllData<Employee, EmployeeEntity>(e => EmployeeEntity.Map.To(e));

            return new OkObjectResult(Response.Generate(employees));
        }

        [FunctionName(nameof(CreateEmployee))]
        //[return: Table(Resource.Table.Employees)]
        public async Task<EmployeeEntity> CreateEmployee(
                [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees")] HttpRequest req,
                ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            var employee = await Request.Parse<EmployeeNew>(req);

            return EmployeeEntity.Map.From(employee);
        }
    }
}
