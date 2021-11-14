using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;
using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Departments;

public class DepartmentPayrollCreateCommandHandler : ICommandHandler<DepartmentPayrollCreateCommand, DepartmentPayroll>
{
    private readonly CosmosClient client;

    public DepartmentPayrollCreateCommandHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryAsync<DepartmentPayroll> Execute(DepartmentPayrollCreateCommand command, CancellationToken token) =>
        DepartmentPayrollRecord
            .Map
            .CreateNewFrom(command.Employee, command.RecordId, command.EmployeePayroll)
            .Apply(record => client
                .GetDepartmentsContainer()
                .CreateItemAsync(record, cancellationToken: token))
            .Apply(TryAsync)
            .Map(CosmosResponse.Unwrap)
            .Map(DepartmentPayrollRecord.Map.ToDepartmentPayroll);
}
