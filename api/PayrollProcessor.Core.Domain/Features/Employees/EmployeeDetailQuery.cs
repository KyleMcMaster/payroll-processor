using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Employees;

public class EmployeeDetailQuery : IQuery<EmployeeDetail>
{
    public Guid EmployeeId { get; }

    public EmployeeDetailQuery(Guid employeeId)
    {
        Guard.Against.Default(employeeId, nameof(employeeId));

        EmployeeId = employeeId;
    }
}
