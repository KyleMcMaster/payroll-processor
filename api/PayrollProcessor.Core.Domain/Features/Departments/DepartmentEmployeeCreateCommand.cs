using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Departments;

public class DepartmentEmployeeCreateCommand : ICommand<DepartmentEmployee>
{
    public Employee Employee { get; }
    public Guid RecordId { get; }

    public DepartmentEmployeeCreateCommand(Employee employee, Guid recordId)
    {
        Guard.Against.Null(employee, nameof(employee));
        Guard.Against.Default(recordId, nameof(recordId));

        this.RecordId = recordId;
        this.Employee = employee;
    }
}
