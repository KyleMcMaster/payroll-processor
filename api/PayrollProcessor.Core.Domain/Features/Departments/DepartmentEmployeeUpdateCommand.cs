using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Departments
{
    public class DepartmentEmployeeUpdateCommand : ICommand<DepartmentEmployee>
    {
        public Employee Employee { get; }
        public DepartmentEmployee DepartmentEmployee { get; }

        public DepartmentEmployeeUpdateCommand(Employee employee, DepartmentEmployee departmentEmployee)
        {
            Guard.Against.Null(employee, nameof(employee));
            Guard.Against.Null(departmentEmployee, nameof(departmentEmployee));

            Employee = employee;
            DepartmentEmployee = departmentEmployee;
        }
    }
}
