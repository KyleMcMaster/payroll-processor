using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Departments
{
    public class DepartmentPayrollUpdateCommand : ICommand<DepartmentPayroll>
    {
        public Employee Employee { get; }
        public EmployeePayroll EmployeePayroll { get; }
        public DepartmentPayroll DepartmentPayroll { get; }

        public DepartmentPayrollUpdateCommand(Employee employee, EmployeePayroll employeePayroll, DepartmentPayroll departmentPayroll)
        {
            Guard.Against.Null(employee, nameof(employee));
            Guard.Against.Null(employeePayroll, nameof(employeePayroll));
            Guard.Against.Null(departmentPayroll, nameof(departmentPayroll));

            EmployeePayroll = employeePayroll;
            DepartmentPayroll = departmentPayroll;
            Employee = employee;
        }
    }
}
