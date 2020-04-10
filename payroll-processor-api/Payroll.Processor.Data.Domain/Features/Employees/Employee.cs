using System;

namespace Payroll.Processor.Data.Domain.Features.Employees
{
    public class Employee
    {
        public Guid Id { get; }
        public string Department { get; }
        public DateTimeOffset EmploymentStartedOn { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Phone { get; }
        public string Title { get; }

        public Employee(Guid id,
            string department,
            DateTimeOffset employmentStartedOn,
            string firstName,
            string lastName,
            string phone,
            string title)
        {
            Id = id;
            Department = department;
            EmploymentStartedOn = employmentStartedOn;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Title = title;
        }
    }
}
