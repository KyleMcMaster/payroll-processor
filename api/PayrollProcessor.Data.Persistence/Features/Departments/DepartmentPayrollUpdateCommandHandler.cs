using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentPayrollUpdateCommandHandler : ICommandHandler<DepartmentPayrollUpdateCommand, DepartmentPayroll>
    {
        private readonly CosmosClient client;

        public DepartmentPayrollUpdateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<DepartmentPayroll> Execute(DepartmentPayrollUpdateCommand command, CancellationToken token)
        {
            var record = DepartmentPayrollRecord.Map.Merge(command.Employee, command.EmployeePayroll, command.DepartmentPayroll);

            return async () =>
            {
                var response = await client
                    .GetDepartmentsContainer()
                    .ReplaceItemAsync(
                        record, record.Id.ToString(),
                        new PartitionKey(record.PartitionKey),
                        new ItemRequestOptions { IfMatchEtag = record.ETag }, token);

                return DepartmentPayrollRecord.Map.ToDepartmentPayroll(response);
            };
        }
    }
}
