using System;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages
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
