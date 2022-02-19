using System;
using System.Collections.Generic;

namespace PayrollProcessor.Core.Domain.Features.Employees;

public class Employee
{
    public Guid Id { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTimeOffset EmploymentStartedOn { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public Employee(Guid id) => Id = id;
}

public class EmployeeDetail : Employee
{
    public EmployeeDetail(Guid id) : base(id)
    {

    }

    public IEnumerable<EmployeePayroll> Payrolls { get; set; } = new EmployeePayroll[] { };
}
