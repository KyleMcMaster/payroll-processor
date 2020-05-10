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

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentEmployeesQueryHandler : IQueryHandler<DepartmentEmployeesQuery, IEnumerable<DepartmentEmployee>>
    {
        private readonly CosmosClient client;

        public DepartmentEmployeesQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<IEnumerable<DepartmentEmployee>> Execute(DepartmentEmployeesQuery query, CancellationToken token = default)
        {
            var (count, department) = query;

            var dataQuery = client
                .DepartmentQueryable<DepartmentEmployeeRecord>(department)
                .Where(e => e.Type == nameof(DepartmentEmployeeRecord));

            if (count > 0)
            {
                dataQuery = dataQuery.Take(count);
            }

            return async () =>
            {
                var iterator = dataQuery.ToFeedIterator();

                var employees = new List<DepartmentEmployee>();

                while (iterator.HasMoreResults)
                {
                    foreach (var result in await iterator.ReadNextAsync(token))
                    {
                        employees.Add(DepartmentEmployeeRecord.Map.ToDepartmentEmployee(result));
                    }
                }

                return Some(employees.AsEnumerable());
            };
        }
    }
}
