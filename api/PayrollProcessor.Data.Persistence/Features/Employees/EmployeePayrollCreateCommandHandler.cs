using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Data.Domain.Features.Employees;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands;

using static LanguageExt.Prelude;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeePayrollCreateCommandHandler : ICommandHandler<EmployeePayrollCreateCommand, Exception>
    {
        private readonly CosmosClient client;

        public EmployeePayrollCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public async Task<Either<Exception, Unit>> Execute(EmployeePayrollCreateCommand command, CancellationToken token)
        {
            var (employee, newPayrollId, newEmployeePayroll) = command;

            var entity = EmployeePayrollRecord.Map.From(employee, newPayrollId, newEmployeePayroll);

            try
            {
                await client.GetEmployeesContainer().CreateItemAsync(entity);

                return Right(Unit.Default);
            }
            catch (Exception ex)
            {
                return Left(ex);
            }
        }
    }
}
