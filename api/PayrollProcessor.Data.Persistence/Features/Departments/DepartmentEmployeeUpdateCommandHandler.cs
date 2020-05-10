using System.Linq;
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
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

            var departmentsContainer = client.GetDepartmentsContainer();

            return async () =>
            {
                var response = await departmentsContainer
                    .ReplaceItemAsync(
                        record, record.Id.ToString(),
                        new PartitionKey(record.PartitionKey),
                        new ItemRequestOptions { IfMatchEtag = record.ETag }, token);

                var iterator = departmentsContainer
                    .GetItemLinqQueryable<DepartmentPayrollRecord>(requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(record.PartitionKey)
                    })
                    .Where(p => p.Type == nameof(DepartmentPayrollRecord))
                    .Where(p => p.EmployeeId == command.Employee.Id)
                    .ToFeedIterator();

                while (iterator.HasMoreResults)
                {
                    foreach (var result in await iterator.ReadNextAsync(token))
                    {
                        var recordToUpdate = DepartmentPayrollRecord.Map.Merge(command.Employee, result);

                        await departmentsContainer.ReplaceItemAsync(
                            recordToUpdate,
                            recordToUpdate.Id.ToString(),
                            new PartitionKey(recordToUpdate.PartitionKey),
                            new ItemRequestOptions { IfMatchEtag = recordToUpdate.ETag },
                            token);
                    }
                }

                return DepartmentEmployeeRecord.Map.ToDepartmentEmployee(response);
            };
        }
    }
}
