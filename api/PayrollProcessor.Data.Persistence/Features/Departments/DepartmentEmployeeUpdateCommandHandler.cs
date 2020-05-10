using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentEmployeeUpdateCommandHandler : ICommandHandler<DepartmentEmployeeUpdateCommand, DepartmentEmployee>
    {
        private readonly CosmosClient client;

        public DepartmentEmployeeUpdateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<DepartmentEmployee> Execute(DepartmentEmployeeUpdateCommand command, CancellationToken token)
        {
            var record = DepartmentEmployeeRecord.Map.Merge(command.Employee, command.DepartmentEmployee);

            return async () =>
            {
                var response = await client
                    .GetDepartmentsContainer()
                    .ReplaceItemAsync(
                        record, record.Id.ToString(),
                        new PartitionKey(record.PartitionKey),
                        new ItemRequestOptions { IfMatchEtag = record.ETag }, token);

                return DepartmentEmployeeRecord.Map.ToDepartmentEmployee(response);
            };
        }
    }
}
