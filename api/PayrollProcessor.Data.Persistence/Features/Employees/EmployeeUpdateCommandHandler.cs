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
    public class EmployeeUpdateCommandHandler : ICommandHandler<EmployeeUpdateCommand, Employee>
    {
        private readonly CosmosClient client;
        private readonly QueueClient queueClient;

        public EmployeeUpdateCommandHandler(CosmosClient client, IQueueClientFactory clientFactory)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;

            queueClient = clientFactory.Create(AppResources.Queue.EmployeeUpdates);
        }

        public TryAsync<Employee> Execute(EmployeeUpdateCommand command, CancellationToken token) =>
            command
                .Apply(EmployeeRecord.Map.Merge)
                .Apply(record =>
                {
                    string identifier = record.Id.ToString();

                    return client
                        .GetEmployeesContainer()
                        .ReplaceItemAsync(
                            record, identifier,
                            new PartitionKey(identifier),
                            new ItemRequestOptions { IfMatchEtag = record.ETag }, token);
                })
                .Apply(TryAsync)
                .Map(CosmosResponse.Unwrap)
                .SelectMany(record => QueueMessageBuilder
                    .ToQueueMessage(queueClient, new EmployeeUpdate
                    {
                        EmployeeId = command.EntityToUpdate.Id
                    })
                    .Apply(TryAsync),
                    (record, _) => record)
                .Map(EmployeeRecord.Map.ToEmployee);
    }
}
