using System.Threading;
using Ardalis.GuardClauses;
using LanguageExt;
using Microsoft.Azure.Cosmos;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentPayrollCreateCommandHandler : ICommandHandler<DepartmentPayrollCreateCommand, DepartmentPayroll>
    {
        private readonly CosmosClient client;

        public DepartmentPayrollCreateCommandHandler(CosmosClient client)
        {
            Guard.Against.Null(client, nameof(client));

            this.client = client;
        }

        public TryOptionAsync<DepartmentPayroll> Execute(DepartmentPayrollCreateCommand command, CancellationToken token)
        {
            var record = DepartmentPayrollRecord.Map.CreateNewFrom(command.Employee, command.RecordId, command.EmployeePayroll);

            return async () =>
            {
                var response = await client
                    .GetDepartmentsContainer()
                    .CreateItemAsync(record, cancellationToken: token);

                return DepartmentPayrollRecord.Map.ToDepartmentPayroll(response);
            };
        }
    }
}
