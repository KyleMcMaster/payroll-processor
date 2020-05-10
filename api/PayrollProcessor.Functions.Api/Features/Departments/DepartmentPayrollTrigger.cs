using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Functions.Api.Infrastructure;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;
using PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;

namespace PayrollProcessor.Functions.Api.Features.Departments
{
    public class DepartmentPayrollTrigger
    {
        private readonly IApiClient apiClient;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IEntityIdGenerator idGenerator;

        public DepartmentPayrollTrigger(
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

        [FunctionName(nameof(EmployeePayrollUpdatesQueue))]
        public Task EmployeePayrollUpdatesQueue(
            [QueueTrigger(AppResources.Queue.EmployeePayrollUpdates)] CloudQueueMessage queueMessage,
            ILogger log)
        {
            log.LogInformation($"Processing {AppResources.Queue.EmployeeUpdates} queue: [{queueMessage}]");

            string eventName = QueueMessageHandler.GetEventName(queueMessage);

            return eventName switch
            {
                nameof(EmployeePayrollCreation) => HandleEmployeePayrollCreation(queueMessage, log),
                nameof(EmployeePayrollUpdate) => HandleEmployeePayrollUpdate(queueMessage, log),
                _ => Task.CompletedTask,
            };
        }

        private Task HandleEmployeePayrollCreation(CloudQueueMessage queueMessage, ILogger log)
        {
            var (employeeId, employeePayrollId) = QueueMessageHandler.FromQueueMessage<EmployeePayrollCreation>(queueMessage);

            return queryDispatcher.Dispatch(new EmployeeQuery(employeeId))
                .SelectMany(
                    employee => queryDispatcher.Dispatch(new EmployeePayrollQuery(employeeId, employeePayrollId)),
                    (employee, employeePayroll) => new { employee, employeePayroll })
                .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentPayrollCreateCommand(aggregate.employee, idGenerator.Generate(), aggregate.employeePayroll)))
                .Bind(departmentPayroll => apiClient.SendNotification(nameof(EmployeePayrollUpdatesQueue), departmentPayroll))
                .Match(
                    _ => log.LogInformation(""),
                    () => log.LogError(""),
                    ex => log.LogError(ex, "")
                );
        }

        private Task HandleEmployeePayrollUpdate(CloudQueueMessage queueMessage, ILogger log)
        {
            var (employeeId, employeePayrollId) = QueueMessageHandler.FromQueueMessage<EmployeePayrollUpdate>(queueMessage);

            return queryDispatcher.Dispatch(new EmployeeQuery(employeeId))
                .SelectMany(
                    employee => queryDispatcher.Dispatch(new EmployeePayrollQuery(employeeId, employeePayrollId)),
                    (employee, employeePayroll) => new { employee, employeePayroll })
                .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentPayrollCreateCommand(aggregate.employee, idGenerator.Generate(), aggregate.employeePayroll)))
                .Bind(departmentPayroll => apiClient.SendNotification(nameof(EmployeePayrollUpdatesQueue), departmentPayroll))
                .Match(
                    _ => log.LogInformation(""),
                    () => log.LogError(""),
                    ex => log.LogError(ex, "")
                );
        }
    }
}
