using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Data.Domain.Features.Employees
{
    public class EmployeeQuery : IQuery<string, Employee>
    {
        public Guid EmployeeId { get; }

        public EmployeeQuery(Guid employeeId)
        {
            Guard.Against.Default(employeeId, nameof(employeeId));

            EmployeeId = employeeId;
        }
    }
}
