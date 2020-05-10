using System.Threading;
using Ardalis.GuardClauses;
using Azure.Storage.Queues;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

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

            queueClient = clientFactory.Create(AppResources.Queue.EmployeeUpdates);
        }

        public TryOptionAsync<EmployeePayroll> Execute(EmployeePayrollUpdateCommand command, CancellationToken token) =>
            async () =>
            {
                var record = EmployeePayrollRecord.Map.Merge(command); ;

                var updateResponse = await client
                    .GetEmployeesContainer()
                    .ReplaceItemAsync(
                        record, record.Id.ToString(),
                        new PartitionKey(record.PartitionKey),
                        new ItemRequestOptions { IfMatchEtag = record.ETag }, token);

                await QueueMessageBuilder.ToQueueMessage(queueClient, new EmployeePayrollUpdate
                {
                    EmployeeId = command.EntityToUpdate.EmployeeId,
                    EmployeePayrollId = command.EntityToUpdate.Id,
                });

                return EmployeePayrollRecord.Map.ToEmployeePayroll(updateResponse);
            };
    }
}
