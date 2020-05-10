using System.Linq;
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentEmployeeQueryHandler : IQueryHandler<DepartmentEmployeeQuery, DepartmentEmployee>
    {
        private readonly CosmosClient client;

        public DepartmentEmployeeQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<DepartmentEmployee> Execute(DepartmentEmployeeQuery query, CancellationToken token)
        {
            var dataQuery = client.GetDepartmentsContainer()
                .GetItemLinqQueryable<DepartmentEmployeeRecord>(
                    requestOptions: new QueryRequestOptions
                    {
                        PartitionKey = new PartitionKey(query.Department.ToLowerInvariant())
                    })
                .Where(e => e.EmployeeId == query.EmployeeId);

            return async () =>
            {
                var iterator = dataQuery.ToFeedIterator();

                while (iterator.HasMoreResults)
                {
                    foreach (var result in await iterator.ReadNextAsync(token))
                    {
                        return DepartmentEmployeeRecord.Map.ToDepartmentEmployee(result);
                    }
                }

                return None;
            };
        }

    }
}
