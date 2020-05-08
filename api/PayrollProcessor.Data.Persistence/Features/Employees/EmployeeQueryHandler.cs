using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
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
                var entity = await client
                   .GetEmployeesContainer()
                   .ReadItemAsync<EmployeeRecord>(identifier, new PartitionKey(identifier), cancellationToken: token);

                return entity is null
                    ? None
                    : Some(EmployeeRecord.Map.ToEmployee(entity));
            };
        }
    }
}
