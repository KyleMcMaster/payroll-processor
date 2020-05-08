using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Data.Domain.Features.Employees
{
    public class EmployeePayrollQuery : IQuery<EmployeePayroll>
    {
        public Guid EmployeeId { get; }
        public Guid EmployeePayrollId { get; }
        public EmployeePayrollQuery(Guid employeeId, Guid employeePayrollId)
        {
            Guard.Against.Null(employeeId, nameof(employeeId));
            Guard.Against.Null(employeePayrollId, nameof(employeePayrollId));

            EmployeePayrollId = employeePayrollId;
            EmployeeId = employeeId;
        }

        public void Deconstruct(out Guid employeeId, out Guid employeePayrollId)
        {
            employeeId = EmployeeId;
            employeePayrollId = EmployeePayrollId;
        }
    }
}
