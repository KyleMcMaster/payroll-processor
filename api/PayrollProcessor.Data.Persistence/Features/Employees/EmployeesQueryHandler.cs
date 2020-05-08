using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeesQueryHandler : IQueryHandler<EmployeesQuery, IEnumerable<Employee>>
    {
        private readonly CosmosClient client;

        public EmployeesQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<IEnumerable<Employee>> Execute(EmployeesQuery query, CancellationToken token = default)
        {
            var (count, firstName, lastName) = query;

            var dataQuery = client
                .EmployeesQueryable()
                .Where(e => e.Type == "EmployeeEntity");

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                dataQuery = dataQuery.Where(e => e.FirstNameLower.Contains(firstName.ToLowerInvariant()));
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                dataQuery = dataQuery.Where(e => e.LastNameLower.Contains(lastName.ToLowerInvariant()));
            }

            if (count > 0)
            {
                dataQuery = dataQuery.Take(count);
            }

            return async () =>
            {
                var iterator = dataQuery.ToFeedIterator();

                var employees = new List<Employee>();

                while (iterator.HasMoreResults)
                {
                    foreach (var result in await iterator.ReadNextAsync())
                    {
                        employees.Add(EmployeeRecord.Map.ToEmployee(result));
                    }
                }

                return Some(employees.AsEnumerable());
            };
        }
    }
}
