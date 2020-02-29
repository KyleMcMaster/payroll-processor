using System;
using System.Collections.Generic;

namespace PayrollProcessor.Functions
{
    public class Employee
    {
        public Guid Id { get; set; }
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public static IEnumerable<Employee> GetEmployees() =>
            new Employee[]
            {
                new Employee
                {
                    Id = Guid.NewGuid(),
                    EmploymentStartedOn = DateTimeOffset.UtcNow,
                    FirstName = "Nathan",
                    LastName = "Harper",
                    Phone = "123456789"
                },
                new Employee
                {
                    Id = Guid.NewGuid(),
                    EmploymentStartedOn = DateTimeOffset.UtcNow,
                    FirstName = "Kyle",
                    LastName = "McMaster",
                    Phone = "012345678"
                }
            };
    }
}
