using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Data.Domain.Features.Employees
{
    public class EmployeeDetailQuery : IQuery<EmployeeDetail>
    {
        public Guid EmployeeId { get; }

        public EmployeeDetailQuery(Guid employeeId)
        {
            Guard.Against.Default(employeeId, nameof(employeeId));

            EmployeeId = employeeId;
        }
    }
}
