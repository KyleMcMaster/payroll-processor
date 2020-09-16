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
    public class EmployeesCreateCommandHandler : ICommandHandler<EmployeeCreateCommand, Employee>
    {
        private readonly CosmosClient client;
        private readonly QueueClient queueClient;

        public EmployeesCreateCommandHandler(CosmosClient client, IQueueClientFactory clientFactory)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;

            queueClient = clientFactory.Create(AppResources.Queue.EmployeeUpdates);
        }

        public TryAsync<Employee> Execute(EmployeeCreateCommand command, CancellationToken token) =>
            EmployeeRecord
                .Map
                .From(command.NewId, command.Employee)
                .Apply(record => client
                    .GetEmployeesContainer()
                    .CreateItemAsync(record, cancellationToken: token))
                .Apply(TryAsync)
                .Map(CosmosResponse.Unwrap)
                .SelectMany(record => QueueMessageBuilder
                    .ToQueueMessage(queueClient, new EmployeeCreation
                    {
                        EmployeeId = command.NewId
                    })
                    .Apply(TryAsync),
                    (record, _) => record)
                .Map(EmployeeRecord.Map.ToEmployee);
    }
}
