using System;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Employees.QueueMessages
{
    public class EmployeeCreation : IMessage
    {
        public Guid EmployeeId { get; set; }
        public string Source { get; set; } = "";

        public void Deconstruct(out Guid employeeId, out string source)
        {
            employeeId = EmployeeId;
            source = Source;
        }
    }
}
