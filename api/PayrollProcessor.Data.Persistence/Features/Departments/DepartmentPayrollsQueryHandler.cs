using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Departments;

public class DepartmentPayrollsQueryHandler : IQueryHandler<DepartmentPayrollsQuery, IEnumerable<DepartmentPayroll>>
{
    private readonly CosmosClient client;

    public DepartmentPayrollsQueryHandler(CosmosClient client)
    {
        Guard.Against.Null(client, nameof(client));

        this.client = client;
    }

    public TryOptionAsync<IEnumerable<DepartmentPayroll>> Execute(DepartmentPayrollsQuery query, CancellationToken token = default)
    {
        var (count, department, checkDateFrom, checkDateTo) = query;

        var dataQuery = client
            .DepartmentQueryable<DepartmentPayrollRecord>(department)
            .Where(e => e.Type == nameof(DepartmentPayrollRecord));

        if (checkDateFrom.HasValue)
        {
            dataQuery = dataQuery.Where(p => p.CheckDate >= checkDateFrom);
        }

        if (checkDateTo.HasValue)
        {
            dataQuery = dataQuery.Where(p => p.CheckDate <= checkDateTo);
        }

        if (count > 0)
        {
            dataQuery = dataQuery.Take(count);
        }

        return async () =>
        {
            var iterator = dataQuery.ToFeedIterator();

            var employees = new List<DepartmentPayroll>();

            while (iterator.HasMoreResults)
            {
                foreach (var result in await iterator.ReadNextAsync(token))
                {
                    employees.Add(DepartmentPayrollRecord.Map.ToDepartmentPayroll(result));
                }
            }

            return Some(employees.AsEnumerable());
        };
    }
}
