using System;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages
{
    public class EmployeeUpdate : IMessage
    {
        public string Department { get; set; } = "";
        public Guid EmployeeId { get; set; }
        public string EventName { get; set; } = nameof(EmployeeUpdate);

        public void Deconstruct(out string department, out Guid employeeId)
        {
            department = Department;
            employeeId = EmployeeId;
        }
    }
}
