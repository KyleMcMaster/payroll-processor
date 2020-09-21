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

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeePayrollUpdateCommandHandler : ICommandHandler<EmployeePayrollUpdateCommand, EmployeePayroll>
    {
        private readonly CosmosClient client;
        private readonly QueueClient queueClient;

        public EmployeePayrollUpdateCommandHandler(CosmosClient client, IQueueClientFactory clientFactory)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;

            queueClient = clientFactory.Create(AppResources.Queue.EmployeePayrollUpdates);
        }

        public TryAsync<EmployeePayroll> Execute(EmployeePayrollUpdateCommand command, CancellationToken token) =>
            command.Apply(EmployeePayrollRecord.Map.Merge)
                .Apply(record => client
                    .GetEmployeesContainer()
                    .ReplaceItemAsync(
                        record, record.Id.ToString(),
                        new PartitionKey(record.PartitionKey),
                        new ItemRequestOptions { IfMatchEtag = record.ETag }, token))
                .Apply(TryAsync)
                .Map(CosmosResponse.Unwrap)
                .SelectMany(record => QueueMessageBuilder
                    .ToQueueMessage(queueClient, new EmployeePayrollUpdate
                    {
                        EmployeeId = command.EntityToUpdate.EmployeeId,
                        EmployeePayrollId = command.EntityToUpdate.Id,
                    })
                    .Apply(TryAsync),
                    (record, _) => record)
                .Map(EmployeePayrollRecord.Map.ToEmployeePayroll);
    }
}
