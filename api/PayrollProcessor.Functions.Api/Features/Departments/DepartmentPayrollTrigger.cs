using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Queue;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Identifiers;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;
using PayrollProcessor.Functions.Api.Infrastructure;

namespace PayrollProcessor.Functions.Api.Features.Departments;

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
    public async Task EmployeePayrollUpdatesQueue(
        [QueueTrigger(AppResources.Queue.EmployeePayrollUpdates)] CloudQueueMessage queueMessage,
        ILogger log)
    {
        log.LogInformation($"Processing {AppResources.Queue.EmployeeUpdates} queue: [{queueMessage}]");

        string eventName = QueueMessageHandler.GetEventName(queueMessage);

        switch (eventName)
        {
            case nameof(EmployeePayrollCreation):
                await HandleEmployeePayrollCreation(queueMessage, log);

                return;

            case nameof(EmployeePayrollUpdate):
                await HandleEmployeePayrollUpdate(queueMessage, log);

                return;

            default:
                log.LogWarning("Queue message with {eventName} cannot be handled", eventName);

                return;
        }
    }

    private Task HandleEmployeePayrollCreation(CloudQueueMessage queueMessage, ILogger log) =>
        queueMessage
            .Apply(QueueMessageHandler.FromQueueMessage<EmployeePayrollCreation>)
            .Apply(message => queryDispatcher.Dispatch(new EmployeeQuery(message.EmployeeId))
                .DoIfNoneOrFail(
                    () => log.LogError("Could not find Employee {employeeId}", message.EmployeeId),
                    ex => log.LogError(ex, "Could not query for Employee {employeeId}", message.EmployeeId)
                )
                .SelectMany(
                    employee => queryDispatcher.Dispatch(new EmployeePayrollQuery(employee.Id, message.EmployeePayrollId))
                        .DoIfNoneOrFail(
                            () => log.LogError("Could not find Employee Payroll for {employeeId} and {employeePayrollId}", employee.Id, message.EmployeePayrollId),
                            ex => log.LogError(ex, "Could not query for Employee Payroll for {employeeId} and {employeePayrollId}", employee.Id, message.EmployeePayrollId)),
                    (employee, employeePayroll) => new { employee, employeePayroll })
            )
            .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentPayrollCreateCommand(aggregate.employee, idGenerator.Generate(), aggregate.employeePayroll))
                .DoIfFail(ex => log.LogError(ex, "Could not create Department Payroll for {@employee} and {@payroll}", aggregate.employee, aggregate.employeePayroll))
                .Do(dp => log.LogInformation("{@departmentPayroll} created for {employeeId} {employeePayrollId}", dp, aggregate.employee.Id, aggregate.employeePayroll.Id))
                .ToTryOption())
            .Bind(departmentPayroll => apiClient.SendNotification(nameof(EmployeePayrollUpdatesQueue), departmentPayroll)
                .DoIfFail(ex => log.LogError(ex, "Could not send API notification for {@departmentPayroll} creation", departmentPayroll))
                .Do(_ => log.LogInformation($"API notification sent")))
            .Try();

    private Task HandleEmployeePayrollUpdate(CloudQueueMessage queueMessage, ILogger log) =>
        queueMessage
            .Apply(QueueMessageHandler.FromQueueMessage<EmployeePayrollUpdate>)
            .Apply(message => queryDispatcher.Dispatch(new EmployeeQuery(message.EmployeeId))
                .DoIfNoneOrFail(
                    () => log.LogError("Could not find Employee {employeeId}", message.EmployeeId),
                    ex => log.LogError(ex, "Could not query for Employee {employeeId}", message.EmployeeId)
                )
                .SelectMany(
                    employee => queryDispatcher.Dispatch(new EmployeePayrollQuery(message.EmployeeId, message.EmployeePayrollId))
                        .DoIfNoneOrFail(
                            () => log.LogError("Could not find Employee Payroll for {employeeId} and {employeePayrollId}", employee.Id, message.EmployeePayrollId),
                            ex => log.LogError(ex, "Could not query for Employee Payroll for {employeeId} and {employeePayrollId}", employee.Id, message.EmployeePayrollId)),
                    (employee, employeePayroll) => new { employee, employeePayroll }))
            .SelectMany(
                aggregate =>
                {
                    var query = new DepartmentPayrollQuery(aggregate.employee.Department, aggregate.employeePayroll.Id);

                    return queryDispatcher
                        .Dispatch(query)
                        .DoIfNoneOrFail(
                            () => log.LogError("Could not find Department Payroll for {@query}", query),
                            ex => log.LogError(ex, "Could not query for Department Payroll for {@query}", query));
                },
                (a, b) => new { a.employee, a.employeePayroll, departmentEmployee = b })
            .Bind(aggregate => commandDispatcher.Dispatch(new DepartmentPayrollUpdateCommand(aggregate.employee, aggregate.employeePayroll, aggregate.departmentEmployee))
                .DoIfFail(ex => log.LogError(ex, "Could not update Department Payroll for {@employee} and {@payroll}", aggregate.employee, aggregate.employeePayroll))
                .Do(dp => log.LogInformation("{@departmentPayroll} updated", dp))
                .ToTryOption())
            .Bind(departmentPayroll => apiClient.SendNotification(nameof(EmployeePayrollUpdatesQueue), departmentPayroll)
                .DoIfFail(ex => log.LogError(ex, "Could not send API notification for {@departmentPayroll} update", departmentPayroll))
                .Do(_ => log.LogInformation($"API notification sent")))
            .Try();
}
