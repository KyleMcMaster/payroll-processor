using System;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeeRecord
    {
        public Guid Id { get; set; }
        public string Department { get; set; }
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Title { get; set; }
    }
}
