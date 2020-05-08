
using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeePayrollCreateCommandHandler : ICommandHandler<EmployeePayrollCreateCommand, EmployeePayroll>
    {
        private readonly CosmosClient client;

        public EmployeePayrollCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<EmployeePayroll> Execute(EmployeePayrollCreateCommand command, CancellationToken token)
        {
            var (employee, newPayrollId, newEmployeePayroll) = command;

            var entity = EmployeePayrollRecord.Map.From(employee, newPayrollId, newEmployeePayroll);

            return async () =>
            {
                var response = await client.GetEmployeesContainer().CreateItemAsync(entity);

                return EmployeePayrollRecord.Map.ToEmployeePayroll(response);
            };
        }
    }
}
