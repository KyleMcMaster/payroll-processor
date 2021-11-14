using System;
using PayrollProcessor.Data.Persistence.Infrastructure.Clients;

namespace PayrollProcessor.Data.Persistence.Features.Employees.QueueMessages;

public class EmployeeCreation : IMessage
{
    public Guid EmployeeId { get; set; }
    public string EventName { get; set; } = nameof(EmployeeCreation);
}
