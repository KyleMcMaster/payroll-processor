using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Departments;

public class DepartmentEmployeesQuery : IQuery<IEnumerable<DepartmentEmployee>>
{
    public string Department { get; }
    public int Count { get; }

    public DepartmentEmployeesQuery(int count, string department)
    {
        Guard.Against.NullOrWhiteSpace(department, nameof(department));

        Count = count;
        Department = department;
    }

    public void Deconstruct(out int count, out string department)
    {
        count = Count;
        department = Department;
    }
}
