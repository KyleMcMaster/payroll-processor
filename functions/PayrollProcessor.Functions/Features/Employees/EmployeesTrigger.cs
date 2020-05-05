using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using PayrollProcessor.Core.Domain.Features.Employees;
using System.Linq;
using PayrollProcessor.Functions.Infrastructure;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Functions.Features.Employees.QueueMessages;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeesTrigger
    {
        private readonly IEmployeesQueryHandler employeesQueryHandler;
        private readonly IEmployeeCreateCommandHandler employeeCreateCommandHandler;
        private readonly IEmployeeUpdateCommandHandler employeeUpdateCommandHandler;
        private readonly IEmployeePayrollCreateCommandHandler employeePayrollCreateCommandHandler;

        public EmployeesTrigger(
            IEmployeesQueryHandler employeesQueryHandler,
            IEmployeeCreateCommandHandler employeeCreateCommandHandler,
            IEmployeeUpdateCommandHandler employeeUpdateCommandHandler,
            IEmployeePayrollCreateCommandHandler employeePayrollCreateCommandHandler)
        {
            this.employeesQueryHandler = employeesQueryHandler ?? throw new ArgumentNullException(nameof(employeesQueryHandler));
            this.employeeCreateCommandHandler = employeeCreateCommandHandler ?? throw new ArgumentNullException(nameof(employeeCreateCommandHandler));
            this.employeeUpdateCommandHandler = employeeUpdateCommandHandler ?? throw new ArgumentNullException(nameof(employeeUpdateCommandHandler));
            this.employeePayrollCreateCommandHandler = employeePayrollCreateCommandHandler ?? throw new ArgumentNullException(nameof(employeePayrollCreateCommandHandler));
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

            var employees = await employeesQueryHandler.GetMany(count, firstName, lastName, email);

            return employees.ToArray();
        }

        [FunctionName(nameof(GetEmployee))]
        public async Task<ActionResult<Employee>> GetEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "employees/{employeeId}")] HttpRequest req,
            Guid employeeId,
            ILogger log)
        {
            log.LogInformation($"Retrieving employee details: [{req}]");

            var option = await employeesQueryHandler.GetDetail(employeeId);

            return option.IfNone(() => throw new Exception($"Could not find employee [{employeeId}]"));
        }

        [FunctionName(nameof(CreateEmployee))]
        public async Task<ActionResult<Employee>> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees")] EmployeeNew newEmployee,
            HttpRequest req,
            [Queue(AppResources.Queue.DepartmentUpdates)] CloudQueue departmentUpdatesQueue,
            ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            var employee = await employeeCreateCommandHandler.Execute(newEmployee);

            var message = new EmployeeCreation
            {
                EmployeeId = employee.Id,
                Source = nameof(CreateEmployee)
            };

            await departmentUpdatesQueue.AddMessageAsync(QueueMessageFactory.ToQueueMessage(message));

            return employee;
        }

        [FunctionName(nameof(EnableEmployee))]
        public async Task<ActionResult<Employee>> EnableEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees/{employeeId}/enable")] HttpRequest req,
            Guid employeeId,
            ILogger log)
        {
            log.LogInformation($"Enabling employee: [{req}]");

            var employeeOption = await employeesQueryHandler.Get(employeeId);

            var employeeToEnable = employeeOption.IfNone(() => throw new Exception($"Could not find employee [{employeeId}]"));

            if (employeeToEnable.Status == EmployeeStatus.Enabled.CodeName)
            {
                return employeeToEnable;
            }

            employeeToEnable.Status = EmployeeStatus.Enabled.CodeName;

            return await employeeUpdateCommandHandler.Execute(employeeToEnable);
        }

        [FunctionName(nameof(DisableEmployee))]
        public async Task<ActionResult<Employee>> DisableEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees/{employeeId}/disable")] HttpRequest req,
            Guid employeeId,
            ILogger log)
        {
            log.LogInformation($"Disabling employee: [{req}]");

            var employeeOption = await employeesQueryHandler.Get(employeeId);

            var employeeToDisable = employeeOption.IfNone(() => throw new Exception($"Could not find employee [{employeeId}]"));

            if (employeeToDisable.Status == EmployeeStatus.Disabled.CodeName)
            {
                return employeeToDisable;
            }

            employeeToDisable.Status = EmployeeStatus.Disabled.CodeName;

            return await employeeUpdateCommandHandler.Execute(employeeToDisable);
        }

        [FunctionName(nameof(CreateEmployeePayroll))]
        public async Task<ActionResult<EmployeePayroll>> CreateEmployeePayroll(
            [HttpTrigger(AuthorizationLevel.Anonymous, "POST", Route = "employees/{employeeId}/payrolls")] EmployeePayrollNew newEmployeePayroll,
            Guid employeeId,
            [Queue(AppResources.Queue.DepartmentUpdates)] CloudQueue departmentUpdatesQueue,
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Creating a new employee: [{req}]");

            var employeeOption = await employeesQueryHandler.GetDetail(employeeId);

            var employee = employeeOption.IfNone(() => throw new Exception($"Could not find employee [{employeeId}]"));

            var employeePayroll = await employeePayrollCreateCommandHandler.Execute(employee, newEmployeePayroll);

            var message = new EmployeePayrollCreation
            {
                EmployeeId = employee.Id,
                EmployeePayrollId = employeePayroll.Id,
                Source = nameof(CreateEmployeePayroll)
            };

            await departmentUpdatesQueue.AddMessageAsync(QueueMessageFactory.ToQueueMessage(message));

            return employeePayroll;
        }
    }
}
