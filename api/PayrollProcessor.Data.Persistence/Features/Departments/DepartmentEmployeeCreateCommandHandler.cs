using System.Threading;
using Ardalis.GuardClauses;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Departments;

public class DepartmentEmployeeCreateCommandHandler : ICommandHandler<DepartmentEmployeeCreateCommand, DepartmentEmployee>
{
    private readonly CosmosClient client;

    public DepartmentEmployeeCreateCommandHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryAsync<DepartmentEmployee> Execute(DepartmentEmployeeCreateCommand command, CancellationToken token) =>
        DepartmentEmployeeRecord
            .Map
            .CreateNewFrom(command.Employee, command.RecordId)
            .Apply(record => client
                .GetDepartmentsContainer()
                .CreateItemAsync(record, cancellationToken: token))
            .Apply(TryAsync)
            .Map(CosmosResponse.Unwrap)
            .Map(DepartmentEmployeeRecord.Map.ToDepartmentEmployee);
}
