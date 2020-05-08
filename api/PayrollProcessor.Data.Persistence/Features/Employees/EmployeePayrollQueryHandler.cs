using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeePayrollQueryHandler : IQueryHandler<EmployeePayrollQuery, EmployeePayroll>
    {
        private readonly CosmosClient client;

        public EmployeePayrollQueryHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<EmployeePayroll> Execute(EmployeePayrollQuery query, CancellationToken token = default)
        {
            var container = client.GetEmployeesContainer();

            var (employeeId, employeePayrollId) = query;

            return async () =>
            {
                var record = await container.ReadItemAsync<EmployeePayrollRecord>(
                    employeePayrollId.ToString(),
                    new PartitionKey(employeeId.ToString()),
                    cancellationToken: token);

                return record is null
                    ? None
                    : Some(EmployeePayrollRecord.Map.ToEmployeePayroll(record));
            };
        }
    }
}
