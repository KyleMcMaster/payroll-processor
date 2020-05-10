using System.Linq;
using System.Threading;
using Ardalis.GuardClauses;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentPayrollQueryHandler : IQueryHandler<DepartmentPayrollQuery, DepartmentPayroll>
    {
        private readonly CosmosClient client;

        public DepartmentPayrollQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public LanguageExt.TryOptionAsync<DepartmentPayroll> Execute(DepartmentPayrollQuery query, CancellationToken token)
        {
            var dataQuery = client.GetDepartmentsContainer()
                .GetItemLinqQueryable<DepartmentPayrollRecord>(
                    requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(query.Department.ToLowerInvariant())
                    })
                .Where(e => e.EmployeePayrollId == query.EmployeePayrollId);

            return async () =>
            {
                var iterator = dataQuery.ToFeedIterator();

                while (iterator.HasMoreResults)
                {
                    foreach (var result in await iterator.ReadNextAsync(token))
                    {
                        return DepartmentPayrollRecord.Map.ToDepartmentPayroll(result);
                    }
                }

                return None;
            };
        }

    }
}
