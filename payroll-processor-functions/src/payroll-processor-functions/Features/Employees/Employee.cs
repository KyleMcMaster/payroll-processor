using System;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Department { get; set; } = "";
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Title { get; set; } = "";

        public Employee(Guid id)
        {
            Id = id;
        }
    }
}
