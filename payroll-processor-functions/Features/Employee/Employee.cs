using System;

namespace PayrollProcessor.Functions
{
    public class Employee
    {
        public Guid Id { get; set; }
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
    }
}
