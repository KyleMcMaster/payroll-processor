using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Functions.Api.Infrastructure;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Functions.Api.Features.Departments
{
    public class DepartmentEmployeeTrigger
    {
        private readonly IApiClient apiClient;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IEntityIdGenerator idGenerator;

        public DepartmentEmployeeTrigger(
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

        [FunctionName(nameof(EmployeeUpdatesQueue))]
        public async Task EmployeeUpdatesQueue(
            [QueueTrigger(AppResources.Queue.EmployeeUpdates)] CloudQueueMessage queueMessage,
            ILogger log)
        {
            log.LogInformation($"Processing {AppResources.Queue.EmployeeUpdates} queue: [{queueMessage}]");

            string eventName = QueueMessageHandler.GetEventName(queueMessage);

            switch (eventName)
            {
                case nameof(EmployeeCreation):
                    await HandleEmployeeCreation(queueMessage, log);

                    return;

                case nameof(EmployeeUpdate):
                    await HandleEmployeeUpdate(queueMessage, log);

                    return;

                default:
                    return;
            }
        }

        private Task HandleEmployeeCreation(CloudQueueMessage queueMessage, ILogger log)
        {
            var message = QueueMessageHandler.FromQueueMessage<EmployeeCreation>(queueMessage);

            return queryDispatcher.Dispatch(new EmployeeQuery(message.EmployeeId))
                .Bind(employee => commandDispatcher.Dispatch(new DepartmentEmployeeCreateCommand(employee, idGenerator.Generate())))
                .Bind(departmentEmployee => apiClient.SendNotification(nameof(EmployeeUpdatesQueue), departmentEmployee))
                .Match(
                    _ => log.LogInformation(""),
                    () => log.LogError(""),
                    e => log.LogError(e, ""));
        }

        private Task HandleEmployeeUpdate(CloudQueueMessage queueMessage, ILogger log)
        {
            var message = QueueMessageHandler.FromQueueMessage<EmployeeUpdate>(queueMessage);

            return queryDispatcher.Dispatch(new EmployeeQuery(message.EmployeeId))
                .SelectMany(
                    employee => queryDispatcher.Dispatch(new DepartmentEmployeeQuery(employee.Department, employee.Id)),
                    (employee, departmentEmployee) => new { employee, departmentEmployee })
                .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentEmployeeUpdateCommand(aggregate.employee, aggregate.departmentEmployee)))
                .Bind(departmentEmployee => apiClient.SendNotification(nameof(EmployeeUpdatesQueue), departmentEmployee))
                .Match(
                    _ => log.LogInformation(""),
                    () => log.LogError(""),
                    e => log.LogError(e, ""));
        }
    }
}
