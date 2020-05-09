using System.Collections.Generic;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Queries;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class EmployeesQuery : IQuery<IEnumerable<Employee>>
    {
        public int Count { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public EmployeesQuery(int count, string email, string firstName, string lastName)
        {
            Email = email;
            LastName = lastName;
            FirstName = firstName;
            Count = count;

        }

        public void Deconstruct(out int count, out string email, out string firstName, out string lastName)
        {
            count = Count;
            email = Email;
            firstName = FirstName;
            lastName = LastName;
        }
    }
}
