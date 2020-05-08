using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Data.Domain.Features.Employees
{
    public class EmployeeCreateCommand : ICommand<Employee>
    {
        public Guid NewId { get; set; }
        public EmployeeNew Employee { get; }

        public EmployeeCreateCommand(Guid newId, EmployeeNew employee)
        {
            Guard.Against.Default(newId, nameof(newId));
            Guard.Against.Null(employee, nameof(employee));

            NewId = newId;
            Employee = employee;
        }
    }
}
