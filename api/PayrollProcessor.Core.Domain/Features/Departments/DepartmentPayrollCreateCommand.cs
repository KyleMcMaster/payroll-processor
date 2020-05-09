using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Departments
{
    public class DepartmentPayrollCreateCommand : ICommand<DepartmentPayroll>
    {
        public Employee Employee { get; }
        public EmployeePayroll EmployeePayroll { get; }
        public Guid RecordId { get; }

        public DepartmentPayrollCreateCommand(Employee employee, Guid recordId, EmployeePayroll employeePayroll)
        {
            Guard.Against.Default(recordId, nameof(recordId));
            Guard.Against.Null(employee, nameof(employee));
            Guard.Against.Null(employeePayroll, nameof(employeePayroll));

            RecordId = recordId;
            EmployeePayroll = employeePayroll;
            Employee = employee;
        }
    }
}
