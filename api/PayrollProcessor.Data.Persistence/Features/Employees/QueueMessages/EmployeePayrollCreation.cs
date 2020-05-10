using System;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages
{
    public class EmployeePayrollCreation : IMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid EmployeePayrollId { get; set; }
        public string EventName { get; set; } = nameof(EmployeePayrollCreation);

        public void Deconstruct(out Guid employeeId, out Guid employeePayrollId)
        {
            employeeId = EmployeeId;
            employeePayrollId = EmployeePayrollId;
        }
    }
}
