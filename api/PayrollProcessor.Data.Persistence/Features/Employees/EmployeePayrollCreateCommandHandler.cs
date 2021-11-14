
using System.Threading;
using Ardalis.GuardClauses;
using Azure.Storage.Queues;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;
using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees;

public class EmployeePayrollCreateCommandHandler : ICommandHandler<EmployeePayrollCreateCommand, EmployeePayroll>
{
    private readonly CosmosClient client;
    private readonly QueueClient queueClient;

    public EmployeePayrollCreateCommandHandler(CosmosClient client, IQueueClientFactory clientFactory)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;

        queueClient = clientFactory.Create(AppResources.Queue.EmployeePayrollUpdates);
    }

    public TryAsync<EmployeePayroll> Execute(EmployeePayrollCreateCommand command, CancellationToken token)
    {
        var (employee, newPayrollId, newEmployeePayroll) = command;

        return EmployeePayrollRecord
            .Map
            .From(employee, newPayrollId, newEmployeePayroll)
            .Apply(record => client.GetEmployeesContainer().CreateItemAsync(record, cancellationToken: token))
            .Apply(TryAsync)
            .Map(CosmosResponse.Unwrap)
            .SelectMany(_ => QueueMessageBuilder
                .ToQueueMessage(queueClient, new EmployeePayrollCreation
                {
                    EmployeeId = employee.Id,
                    EmployeePayrollId = newPayrollId,
                })
                .Apply(TryAsync),
                (record, _) => record)
            .Map(EmployeePayrollRecord.Map.ToEmployeePayroll);
    }
}
