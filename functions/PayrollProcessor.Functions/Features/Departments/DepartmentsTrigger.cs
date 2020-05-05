using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Functions.Features.Resources;
using PayrollProcessor.Functions.Infrastructure;
using System;
using System.Threading.Tasks;
using System.Linq;
using PayrollProcessor.Functions.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Functions.Features.Employees.QueueMessages;

namespace PayrollProcessor.Functions.Features.Departments
{
    public class DepartmentsTrigger
    {
        private readonly ApiClient apiClient;
        private readonly IDepartmentPayrollsQueryHandler departmentPayrollsQueryHandler;
        private readonly IDepartmentEmployeesQueryHandler departmentEmployeesQueryHandler;
        private readonly IEmployeesQueryHandler employeesQueryHandler;
        private readonly IDepartmentPayrollCreateCommandHandler payrollCreateCommandHandler;
        private readonly IDepartmentEmployeeCreateCommandHandler departmentEmployeeCreateCommandHandler;

        public DepartmentsTrigger(
            ApiClient apiClient,
            IDepartmentPayrollsQueryHandler departmentPayrollsQueryHandler,
            IDepartmentEmployeesQueryHandler departmentEmployeesQueryHandler,
            IEmployeesQueryHandler employeesQueryHandler,
            IDepartmentPayrollCreateCommandHandler payrollCreateCommandHandler,
            IDepartmentEmployeeCreateCommandHandler departmentEmployeeCreateCommandHandler)
        {
            this.apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            this.departmentPayrollsQueryHandler = departmentPayrollsQueryHandler ?? throw new ArgumentNullException(nameof(departmentPayrollsQueryHandler));
            this.departmentEmployeesQueryHandler = departmentEmployeesQueryHandler ?? throw new ArgumentNullException(nameof(departmentEmployeesQueryHandler));
            this.employeesQueryHandler = employeesQueryHandler ?? throw new ArgumentNullException(nameof(employeesQueryHandler));
            this.payrollCreateCommandHandler = payrollCreateCommandHandler ?? throw new ArgumentNullException(nameof(payrollCreateCommandHandler));
            this.departmentEmployeeCreateCommandHandler = departmentEmployeeCreateCommandHandler ?? throw new ArgumentNullException(nameof(departmentEmployeeCreateCommandHandler));
        }

        [FunctionName(nameof(GetDepartmentPayrolls))]
        public async Task<ActionResult<DepartmentPayroll[]>> GetDepartmentPayrolls(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "departments/payrolls")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Retrieving all department payrolls: [{req}]");

            int.TryParse(req.Query["count"], out int count);

            string department = req.Query["department"].FirstOrDefault() ?? "";

            DateTime? startDate = DateTime.TryParse(req.Query["startDate"], out DateTime sdate)
                ? sdate
                : (DateTime?)null;

            DateTime? endDate = DateTime.TryParse(req.Query["endDate"], out DateTime edate)
                ? edate
                : (DateTime?)null;

            var payrolls = await departmentPayrollsQueryHandler.GetMany(count, department, startDate, endDate);

            return payrolls.ToArray();
        }

        [FunctionName(nameof(GetDepartmentEmployees))]
        public async Task<ActionResult<DepartmentEmployee[]>> GetDepartmentEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "departments/employees")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"Retrieving all department employees: [{req}]");

            int.TryParse(req.Query["count"], out int count);

            string department = req.Query["department"].FirstOrDefault() ?? "";

            var employees = await departmentEmployeesQueryHandler.GetMany(count, department);

            return employees.ToArray();
        }

        [FunctionName(nameof(CreateEmployeeFromQueue))]
        public async Task CreateEmployeeFromQueue(
            [QueueTrigger(AppResources.Queue.DepartmentUpdates)] CloudQueueMessage message,
            ILogger log)
        {
            log.LogInformation($"Processing {AppResources.Queue.DepartmentUpdates} queue: [{message}]");

            var (employeeId, source) = QueueMessageFactory.FromQueueMessage<EmployeeCreation>(message);

            var employeeOption = await employeesQueryHandler.GetDetail(employeeId);

            var employee = employeeOption.IfNone(() => throw new Exception($"Could not find employee [{employeeId}]"));

            var departmentEmployee = await departmentEmployeeCreateCommandHandler.Execute(employee);

            await apiClient.SendNotification(
                nameof(CreateEmployeeFromQueue),
                departmentEmployee
            );
        }

        [FunctionName(nameof(CreatePayrollFromQueue))]
        public async Task CreatePayrollFromQueue(
            [QueueTrigger(AppResources.Queue.DepartmentUpdates)] CloudQueueMessage message,
            ILogger log)
        {
            log.LogInformation($"Processing {AppResources.Queue.DepartmentUpdates} queue: [{message}]");

            var (employeeId, employeePayrollId, source) = QueueMessageFactory.FromQueueMessage<EmployeePayrollCreation>(message);

            var employeeOption = await employeesQueryHandler.GetDetail(employeeId);

            var employee = employeeOption.IfNone(() => throw new Exception($"Could not find employee [{employeeId}] for employee payroll [{employeePayrollId}]"));

            var employeePayrollOption = await employeesQueryHandler.GetPayroll(employeeId, employeePayrollId);

            var employeePayroll = employeePayrollOption.IfNone(() => throw new Exception($"Could not find employee payroll [{employeePayrollId}] for employee [{employeeId}]"));

            var departmentPayroll = await payrollCreateCommandHandler.Execute(employee, employeePayroll);

            await apiClient.SendNotification(
                nameof(CreatePayrollFromQueue),
                departmentPayroll
            );
        }
    }
}
