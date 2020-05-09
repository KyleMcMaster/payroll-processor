using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Functions.Features.Resources;
using PayrollProcessor.Functions.Infrastructure;
using System.Threading.Tasks;
using PayrollProcessor.Functions.Features.Employees.QueueMessages;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;

namespace PayrollProcessor.Functions.Features.Departments
{
    public class DepartmentsTrigger
    {
        private readonly IApiClient apiClient;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IEntityIdGenerator idGenerator;

        public DepartmentsTrigger(
            IApiClient apiClient,
            IQueryDispatcher queryDispatcher,
            ICommandDispatcher commandDispatcher,
            IEntityIdGenerator idGenerator)
        {
            Guard.Against.Null(apiClient, nameof(apiClient));
            Guard.Against.Null(queryDispatcher, nameof(queryDispatcher));
            Guard.Against.Null(commandDispatcher, nameof(commandDispatcher));
            Guard.Against.Null(idGenerator, nameof(idGenerator));

            this.apiClient = apiClient;
            this.queryDispatcher = queryDispatcher;
            this.commandDispatcher = commandDispatcher;
            this.idGenerator = idGenerator;
        }

        [FunctionName(nameof(CreateEmployeeFromQueue))]
        public async Task CreateEmployeeFromQueue(
            [QueueTrigger(AppResources.Queue.EmployeeUpdates)] CloudQueueMessage message,
            ILogger log)
        {
            log.LogInformation($"Processing {AppResources.Queue.EmployeeUpdates} queue: [{message}]");

            var (employeeId, source) = QueueMessageHandler.FromQueueMessage<EmployeeCreation>(message);

            await queryDispatcher.Dispatch(new EmployeeQuery(employeeId))
                .Bind(employee => commandDispatcher.Dispatch(new DepartmentEmployeeCreateCommand(employee, idGenerator.Generate())))
                .Bind(departmentEmployee => apiClient.SendNotification(nameof(CreateEmployeeFromQueue), departmentEmployee))
                .Match(
                    _ => log.LogInformation(""),
                    () => log.LogError(""),
                    e => log.LogError(e, ""));
        }

        [FunctionName(nameof(CreatePayrollFromQueue))]
        public async Task CreatePayrollFromQueue(
            [QueueTrigger(AppResources.Queue.EmployeePayrollUpdates)] CloudQueueMessage message,
            ILogger log)
        {
            log.LogInformation($"Processing {AppResources.Queue.EmployeeUpdates} queue: [{message}]");

            var (employeeId, employeePayrollId, source) = QueueMessageHandler.FromQueueMessage<EmployeePayrollCreation>(message);

            await queryDispatcher.Dispatch(new EmployeeQuery(employeeId))
                .SelectMany(
                    employee => queryDispatcher.Dispatch(new EmployeePayrollQuery(employeeId, employeePayrollId)),
                    (employee, employeePayroll) => new { employee, employeePayroll })
                .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentPayrollCreateCommand(aggregate.employee, idGenerator.Generate(), aggregate.employeePayroll)))
                .Bind(departmentPayroll => apiClient.SendNotification(nameof(CreatePayrollFromQueue), departmentPayroll))
                .Match(
                    _ => log.LogInformation(""),
                    () => log.LogError(""),
                    ex => log.LogError(ex, "")
                );
        }
    }
}
