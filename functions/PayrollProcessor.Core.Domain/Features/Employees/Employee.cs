using System;
using System.Collections.Generic;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Department { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Title { get; set; } = "";
        public string Version { get; set; } = "";

        public Employee(Guid id) => Id = id;
    }

    public class EmployeeDetails : Employee
    {
        public EmployeeDetails(Guid id) : base(id)
        {

        }

        public IEnumerable<EmployeePayroll> Payrolls { get; set; } = new EmployeePayroll[] { };
    }
}
