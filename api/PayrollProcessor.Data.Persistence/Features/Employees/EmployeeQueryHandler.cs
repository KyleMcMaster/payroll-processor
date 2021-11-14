using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees;

public class EmployeeQueryHandler : IQueryHandler<EmployeeQuery, Employee>
{
    private readonly CosmosClient client;

    public EmployeeQueryHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryOptionAsync<Employee> Execute(EmployeeQuery query, CancellationToken token = default)
    {
        string identifier = query.EmployeeId.ToString();

        return async () =>
        {
            try
            {
                var record = await client
                   .GetEmployeesContainer()
                   .ReadItemAsync<EmployeeRecord>(identifier, new PartitionKey(identifier), cancellationToken: token);

                return EmployeeRecord.Map.ToEmployee(record);
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return None;
            }
        };
    }
}
