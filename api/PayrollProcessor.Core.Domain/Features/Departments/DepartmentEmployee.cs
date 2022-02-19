using System;

namespace PayrollProcessor.Core.Domain.Features.Departments;

public class DepartmentEmployee
{
    public DepartmentEmployee(Guid id) => Id = id;

    public Guid Id { get; }
    public Guid EmployeeId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}
