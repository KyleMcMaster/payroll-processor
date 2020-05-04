using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using PayrollProcessor.Core.Domain.Features.Employees;
using System.Linq;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeesTrigger
    {
        private readonly IEmployeesQueryHandler queryHandler;
        private readonly IEmployeeCreateCommandHandler commandHandler;

        public EmployeesTrigger(IEmployeesQueryHandler queryHandler, IEmployeeCreateCommandHandler commandHandler)
        {
            this.queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
            this.commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
        }

        [FunctionName(nameof(GetEmployees))]
        public async Task<ActionResult<Employee[]>> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Retrieving all employees: [{req}]");

            int.TryParse(req.Query["count"], out int count);
            string firstName = req.Query["firstName"].FirstOrDefault() ?? "";
            string lastName = req.Query["lastName"].FirstOrDefault() ?? "";
            string email = req.Query["email"].FirstOrDefault() ?? "";

            var employees = await queryHandler.GetMany(count, firstName, lastName, email);

            return employees.ToArray();
        }

        [FunctionName(nameof(GetEmployee))]
        public async Task<ActionResult<Employee>> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees/{employeeId}")] HttpRequest req,
            Guid employeeId,
            ILogger log)
        {
            log.LogInformation($"Retrieving employee details: [{req}]");

            var option = await queryHandler.Get(employeeId);

            return option.IfNone(() => throw new Exception($"Could not find employee [{employeeId}]"));
        }

        [FunctionName(nameof(CreateEmployee))]
        public async Task<ActionResult<Employee>> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees")] EmployeeNew newEmployee,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            return await commandHandler.Execute(newEmployee);
        }
    }
}
