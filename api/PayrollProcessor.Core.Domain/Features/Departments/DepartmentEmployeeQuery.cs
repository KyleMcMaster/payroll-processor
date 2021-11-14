using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Departments;

public class DepartmentEmployeeQuery : IQuery<DepartmentEmployee>
{
    public string Department { get; }
    public Guid EmployeeId { get; }
    public DepartmentEmployeeQuery(string department, Guid employeeId)
    {
        Guard.Against.NullOrWhiteSpace(department, nameof(department));
        Guard.Against.Default(employeeId, nameof(employeeId));

        EmployeeId = employeeId;
        Department = department;
    }
}
