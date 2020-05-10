using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class EmployeeQuery : IQuery<Employee>
    {
        public Guid EmployeeId { get; }

        public EmployeeQuery(Guid employeeId)
        {
            Guard.Against.Default(employeeId, nameof(employeeId));

            EmployeeId = employeeId;
        }
    }
}
