using System;
using System.Collections.Generic;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Departments
{
    public class DepartmentPayrollsQuery : IQuery<IEnumerable<DepartmentPayroll>>
    {
        public int Count { get; }
        public string Department { get; }
        public DateTime? CheckDateFrom { get; }
        public DateTime? CheckDateTo { get; }

        public DepartmentPayrollsQuery(
            int count,
            string department,
            DateTime? checkDateFrom,
            DateTime? checkDateTo)
        {
            Guard.Against.NullOrWhiteSpace(department, nameof(department));

            Department = department;
            CheckDateFrom = checkDateFrom;
            CheckDateTo = checkDateTo;
            Count = count;
        }

        public void Deconstruct(out int count, out string department, out DateTime? checkDateFrom, out DateTime? checkDateTo)
        {
            count = Count;
            department = Department;
            checkDateFrom = CheckDateFrom;
            checkDateTo = CheckDateTo;
        }
    }
}
