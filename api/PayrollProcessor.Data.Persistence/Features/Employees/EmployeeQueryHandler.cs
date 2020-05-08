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
    public class EmployeeQueryHandler : IQueryHandler<EmployeeQuery, string, Employee>
    {
        private readonly CosmosClient client;

        public EmployeeQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public async Task<Either<string, Employee>> Execute(EmployeeQuery query, CancellationToken token = default)
        {
            string identifier = query.EmployeeId.ToString();

            try
            {
                var entity = await client
                    .GetEmployeesContainer()
                    .ReadItemAsync<EmployeeRecord>(identifier, new PartitionKey(identifier));

                if (entity is null)
                {
                    return Left($"Could not find employee [{query.EmployeeId}]");
                }

                return Right(EmployeeRecord.Map.ToEmployee(entity));
            }
            catch (Exception ex)
            {
                return Left(ex.Message);
            }
        }
    }
}
