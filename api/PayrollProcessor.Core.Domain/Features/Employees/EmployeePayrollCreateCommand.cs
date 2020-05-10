using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class EmployeePayrollCreateCommand : ICommand<EmployeePayroll>
    {
        public Employee Employee { get; }
        public EmployeePayrollNew NewPayroll { get; }
        public Guid NewPayrollId { get; }

        public EmployeePayrollCreateCommand(Employee employee, Guid newPayrollId, EmployeePayrollNew newPayroll)
        {
            Guard.Against.Null(employee, nameof(employee));
            Guard.Against.Null(newPayroll, nameof(newPayroll));
            Guard.Against.Default(newPayrollId, nameof(newPayrollId));

            NewPayroll = newPayroll;
            NewPayrollId = newPayrollId;
            Employee = employee;
        }

        public void Deconstruct(out Employee employee, out Guid newPayrollId, out EmployeePayrollNew newPayroll)
        {
            employee = Employee;
            newPayrollId = NewPayrollId;
            newPayroll = NewPayroll;
        }
    }
}
