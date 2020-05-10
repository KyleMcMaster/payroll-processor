using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

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
                try
                {
                    var record = await container.ReadItemAsync<EmployeePayrollRecord>(
                        employeePayrollId.ToString(),
                        new PartitionKey(employeeId.ToString()),
                        cancellationToken: token);
                    return EmployeePayrollRecord.Map.ToEmployeePayroll(record);
                }
                catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return None;
                }
            };
        }
    }
}
