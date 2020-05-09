
using System.Threading;
using Ardalis.GuardClauses;
using Azure.Storage.Queues;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeePayrollCreateCommandHandler : ICommandHandler<EmployeePayrollCreateCommand, EmployeePayroll>
    {
        private readonly CosmosClient client;
        private readonly QueueClient queueClient;

        public EmployeePayrollCreateCommandHandler(CosmosClient client, IQueueClientFactory clientFactory)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;

            queueClient = clientFactory.Create("");
        }

        public TryOptionAsync<EmployeePayroll> Execute(EmployeePayrollCreateCommand command, CancellationToken token)
        {
            var (employee, newPayrollId, newEmployeePayroll) = command;

            var entity = EmployeePayrollRecord.Map.From(employee, newPayrollId, newEmployeePayroll);

            return async () =>
            {
                var response = await client.GetEmployeesContainer().CreateItemAsync(entity, cancellationToken: token);

                await queueClient.SendMessageAsync("");

                return EmployeePayrollRecord.Map.ToEmployeePayroll(response);
            };
        }
    }
}
