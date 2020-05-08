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
    public class EmployeesCreateCommandHandler : ICommandHandler<EmployeeCreateCommand, Exception>
    {
        private readonly CosmosClient client;

        public EmployeesCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public async Task<Either<Exception, Unit>> Execute(EmployeeCreateCommand command, CancellationToken token)
        {
            var record = EmployeeRecord.Map.From(command.Employee);

            try
            {
                await client.GetEmployeesContainer().CreateItemAsync(record);

                return Right(Unit.Default);
            }
            catch (Exception ex)
            {
                return Left(ex);
            }
        }
    }
}
