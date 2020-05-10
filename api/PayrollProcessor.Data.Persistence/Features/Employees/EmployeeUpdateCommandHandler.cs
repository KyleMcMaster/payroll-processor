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

        public TryOptionAsync<Employee> Execute(EmployeeUpdateCommand command, CancellationToken token) =>
            async () =>
            {
                var record = EmployeeRecord.Map.Merge(command);

                string identifier = record.Id.ToString();

                var updateResponse = await client
                    .GetEmployeesContainer()
                    .ReplaceItemAsync(
                        record, identifier,
                        new PartitionKey(identifier),
                        new ItemRequestOptions { IfMatchEtag = record.ETag }, token);

                await QueueMessageBuilder.ToQueueMessage(queueClient, new EmployeeUpdate
                {
                    EmployeeId = command.EntityToUpdate.Id
                });

                return EmployeeRecord.Map.ToEmployee(updateResponse);
            };
    }
}
