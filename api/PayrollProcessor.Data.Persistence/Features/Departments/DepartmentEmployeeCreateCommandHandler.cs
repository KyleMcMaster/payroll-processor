using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentEmployeeCreateCommandHandler : ICommandHandler<DepartmentEmployeeCreateCommand, DepartmentEmployee>
    {
        private readonly CosmosClient client;

        public DepartmentEmployeeCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<DepartmentEmployee> Execute(DepartmentEmployeeCreateCommand command, CancellationToken token)
        {
            var record = DepartmentEmployeeRecord.Map.CreateNewFrom(command.Employee, command.RecordId);

            return async () =>
            {
                var response = await client
                    .GetDepartmentsContainer()
                    .CreateItemAsync(record, cancellationToken: token);

                return DepartmentEmployeeRecord.Map.ToDepartmentEmployee(response);
            };
        }
    }
}
