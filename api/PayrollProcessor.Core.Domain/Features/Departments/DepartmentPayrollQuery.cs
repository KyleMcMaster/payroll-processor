using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Departments
{
    public class DepartmentPayrollQuery : IQuery<DepartmentPayroll>
    {
        public string Department { get; }
        public Guid EmployeePayrollId { get; }

        public DepartmentPayrollQuery(string department, Guid employeePayrollId)
        {
            Guard.Against.NullOrWhiteSpace(department, nameof(department));
            Guard.Against.Default(employeePayrollId, nameof(employeePayrollId));

            EmployeePayrollId = employeePayrollId;
            Department = department;
        }
    }
}
