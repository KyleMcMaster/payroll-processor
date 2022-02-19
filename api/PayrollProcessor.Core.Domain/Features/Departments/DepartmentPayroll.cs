using System;

namespace PayrollProcessor.Core.Domain.Features.Departments;

public class DepartmentPayroll
{
    public Guid Id { get; set; }
    public Guid EmployeePayrollId { get; set; }
    public DateTimeOffset CheckDate { get; set; }
    public Guid EmployeeId { get; set; }
    public decimal GrossPayroll { get; set; }
    public string PayrollPeriod { get; set; } = string.Empty;
    public string EmployeeDepartment { get; set; } = string.Empty;
    public string EmployeeFirstName { get; set; } = string.Empty;
    public string EmployeeLastName { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;

    public DepartmentPayroll(Guid id) => Id = id;
}
