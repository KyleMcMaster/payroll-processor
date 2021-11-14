using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Departments;

public class DepartmentEmployeeUpdateCommandHandler : ICommandHandler<DepartmentEmployeeUpdateCommand, DepartmentEmployee>
{
    private readonly CosmosClient client;

    public DepartmentEmployeeUpdateCommandHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryAsync<DepartmentEmployee> Execute(DepartmentEmployeeUpdateCommand command, CancellationToken token)
    {
        var departmentsContainer = client.GetDepartmentsContainer();

        return DepartmentEmployeeRecord
            .Map
            .Merge(command.Employee, command.DepartmentEmployee)
            .Apply(record => departmentsContainer
                .ReplaceItemAsync(
                    record, record.Id.ToString(),
                    new PartitionKey(record.PartitionKey),
                    new ItemRequestOptions { IfMatchEtag = record.ETag }, token)
                .Apply(TryAsync)
            )
            .Map(CosmosResponse.Unwrap)
            .SelectMany(record =>
                departmentsContainer
                    .GetItemLinqQueryable<DepartmentPayrollRecord>(requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(record.PartitionKey)
                    })
                    .Where(p => p.Type == nameof(DepartmentPayrollRecord))
                    .Where(p => p.EmployeeId == command.Employee.Id)
                    .ToFeedIterator()
                    .Apply(TryAsync),
                (record, iterator) => new { record, iterator }
            )
            .MapAsync(async aggregate =>
            {
                var record = aggregate.record;
                var iterator = aggregate.iterator;

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

                return DepartmentEmployeeRecord.Map.ToDepartmentEmployee(record);
            });
    }
}
