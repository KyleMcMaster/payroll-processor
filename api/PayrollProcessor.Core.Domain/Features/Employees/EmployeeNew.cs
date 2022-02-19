using System;

namespace PayrollProcessor.Core.Domain.Features.Employees;

public class EmployeeNew
{
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset EmploymentStartedOn { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
}
