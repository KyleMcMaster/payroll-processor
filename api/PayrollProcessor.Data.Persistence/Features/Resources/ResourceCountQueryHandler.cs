using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;
using PayrollProcessor.Data.Persistence.Features.Employees;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Resources;

public class ResourceCountQueryHandler : IQueryHandler<ResourceCountQuery, ResourceCountQueryResponse>
{
    private readonly CosmosClient client;

    public ResourceCountQueryHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryOptionAsync<ResourceCountQueryResponse> Execute(ResourceCountQuery query, CancellationToken token)
    {
        var employeesCountResult = TryOptionAsync(
            GetResourceCount(new QueryDefinition($"SELECT VALUE COUNT(1) FROM c where c.type = '{nameof(EmployeeRecord)}'"),
            token));

        var payrollsCountResult = TryOptionAsync(
            GetResourceCount(new QueryDefinition($"SELECT VALUE COUNT(1) FROM c where c.type = '{nameof(EmployeePayrollRecord)}'"),
            token));

        return employeesCountResult.SelectMany(
            _ => payrollsCountResult,
            (employeesCount, payrollsCount) => new ResourceCountQueryResponse(employeesCount, payrollsCount));
    }

    public async Task<int> GetResourceCount(QueryDefinition query, CancellationToken token)
    {
        using var iterator = client.GetEmployeesContainer().GetItemQueryIterator<int>(query);

        if (!iterator.HasMoreResults) 
        {
            return 0;
        }

        var response = await iterator.ReadNextAsync(token);

        return response.Resource.First();
    }
}
