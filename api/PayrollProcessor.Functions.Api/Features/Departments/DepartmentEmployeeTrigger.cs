using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
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
using LanguageExt;
using static LanguageExt.Prelude;
using Microsoft.WindowsAzure.Storage.Queue;

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
                    log.LogWarning("Queue message with {eventName} cannot be handled", eventName);

                    return;
            }
        }

        private Task HandleEmployeeCreation(CloudQueueMessage queueMessage, ILogger log) =>
            queueMessage
                .Apply(QueueMessageHandler.FromQueueMessage<EmployeeCreation>)
                .Apply(message => queryDispatcher.Dispatch(new EmployeeQuery(message.EmployeeId))
                    .DoIfNoneOrFail(
                        () => log.LogError("Could not find Employee {employeeId}", message.EmployeeId),
                        ex => log.LogError(ex, "Could not query for Employee {employeeId}", message.EmployeeId))
                    .Bind(
                        employee => commandDispatcher.Dispatch(new DepartmentEmployeeCreateCommand(employee, idGenerator.Generate()))
                            .DoIfFail(ex => log.LogError(ex, "Could not create Department Employee for {@employee}", employee))
                            .Do(de => log.LogInformation("{@departmentEmployee} created for {employeeId}", de, message.EmployeeId))
                            .ToTryOption()
                    )
                )
                .Bind(departmentEmployee =>
                    apiClient.SendNotification(nameof(EmployeeUpdatesQueue), departmentEmployee)
                        .DoIfFail(ex => log.LogError(ex, "Could not send API notification for {@departmentEmployee} creation", departmentEmployee))
                        .Do(_ => log.LogInformation($"API notification sent")))
                .Try();

        private Task HandleEmployeeUpdate(CloudQueueMessage queueMessage, ILogger log) =>
            queueMessage
                .Apply(QueueMessageHandler.FromQueueMessage<EmployeeUpdate>)
                .Apply(message => queryDispatcher.Dispatch(new EmployeeQuery(message.EmployeeId))
                    .DoIfNoneOrFail(
                        () => log.LogError("Could not find {employeeId}", message.EmployeeId),
                        ex => log.LogError(ex, "Could not query for employee {employeeId}", message.EmployeeId)
                    )
                    .SelectMany(
                        employee =>
                        {
                            var query = new DepartmentEmployeeQuery(employee.Department, employee.Id);

                            return queryDispatcher
                                .Dispatch(query)
                                .DoIfNoneOrFail(
                                    () => log.LogError("Could not find Department Employee for {@query}", query),
                                    ex => log.LogError(ex, "Could not query for Department Employee for {@query}", query));
                        },
                        (employee, departmentEmployee) => new { employee, departmentEmployee }))
                .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentEmployeeUpdateCommand(aggregate.employee, aggregate.departmentEmployee))
                    .DoIfFail(ex => log.LogError(ex, "Could not update Department Employee {departmentEmployeeId} from Employee {employeeId}", aggregate.departmentEmployee.Id, aggregate.employee.Id))
                    .Do(de => log.LogInformation("{@departmentEmployee} updated", de))
                    .ToTryOption())
                .Bind(departmentEmployee => apiClient.SendNotification(nameof(EmployeeUpdatesQueue), departmentEmployee)
                    .DoIfFail(ex => log.LogError(ex, "Could not send API Notification"))
                    .Do(_ => log.LogInformation($"API notification sent")))
                .Try();

    }
}
