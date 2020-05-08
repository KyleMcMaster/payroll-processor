using System;
using System.Collections.Generic;
using PayrollProcessor.Data.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Data.Domain.Features.Employees
{
    public class EmployeesQuery : IQuery<Exception, IEnumerable<Employee>>
    {
        public int Count { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public EmployeesQuery(int count, string firstName, string lastName)
        {
            LastName = lastName;
            FirstName = firstName;
            Count = count;

        }

        public void Deconstruct(out int count, out string firstName, out string lastName)
        {
            count = Count;
            firstName = FirstName;
            lastName = LastName;
        }
    }
}
