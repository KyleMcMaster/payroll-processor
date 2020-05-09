using System;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Functions.Api.Features.Employees.QueueMessages
{
    public class EmployeePayrollCreation : IMessage
    {
        public Guid EmployeeId { get; set; }
        public Guid EmployeePayrollId { get; set; }
        public string Source { get; set; } = "";

        public void Deconstruct(out Guid employeeId, out Guid employeePayrollId, out string source)
        {
            employeeId = EmployeeId;
            employeePayrollId = EmployeePayrollId;
            source = Source;
        }
    }
}
