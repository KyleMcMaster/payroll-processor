using System;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;

public class EmployeePayrollUpdate : IMessage
{
    public Guid EmployeeId { get; set; }
    public Guid EmployeePayrollId { get; set; }
    public string EventName { get; set; } = nameof(EmployeePayrollUpdate);

    public void Deconstruct(out Guid employeeId, out Guid employeePayrollId)
    {
        employeeId = EmployeeId;
        employeePayrollId = EmployeePayrollId;
    }
}
