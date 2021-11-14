using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Departments;

public class DepartmentPayrollUpdateCommandHandler : ICommandHandler<DepartmentPayrollUpdateCommand, DepartmentPayroll>
{
    private readonly CosmosClient client;

    public DepartmentPayrollUpdateCommandHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryAsync<DepartmentPayroll> Execute(DepartmentPayrollUpdateCommand command, CancellationToken token) =>
        DepartmentPayrollRecord
            .Map
            .Merge(command.Employee, command.EmployeePayroll, command.DepartmentPayroll)
            .Apply(record => client
                .GetDepartmentsContainer()
                .ReplaceItemAsync(
                    record, record.Id.ToString(),
                    new PartitionKey(record.PartitionKey),
                    new ItemRequestOptions { IfMatchEtag = record.ETag }, token))
            .Apply(TryAsync)
            .Map(CosmosResponse.Unwrap)
            .Map(DepartmentPayrollRecord.Map.ToDepartmentPayroll);
}
