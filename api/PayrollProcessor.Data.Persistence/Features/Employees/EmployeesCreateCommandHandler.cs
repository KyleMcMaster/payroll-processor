using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeesCreateCommandHandler : ICommandHandler<EmployeeCreateCommand, Employee>
    {
        private readonly CosmosClient client;

        public EmployeesCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<Employee> Execute(EmployeeCreateCommand command, CancellationToken token)
        {
            var record = EmployeeRecord.Map.From(command.Employee);

            return async () =>
            {
                var response = await client
                    .GetEmployeesContainer()
                    .CreateItemAsync(record, cancellationToken: token);

                return EmployeeRecord.Map.ToEmployee(response);
            };
        }
    }
}
