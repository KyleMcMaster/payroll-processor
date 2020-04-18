using System;
using System.Collections.Generic;
using System.Linq;

namespace PayrollProcessor.Functions.Features.Employee
{
    public static class EmployeeData
    {
        public static int EmployeeCount => Employees().Count();

        public static IEnumerable<Employee> Employees() =>
            new Employee[]
            {
                new Employee
                {
                    Id = new Guid("429c49ae-4728-48de-927a-68c50c7c80b1"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2015-01-01"),
                    FirstName = "Nathan",
                    LastName = "Harper",
                    Phone = "1234567890",
                    Status = "ACTIVE",
                    Title = "CIO"
                },
                new Employee
                {
                    Id = new Guid("50b7eac5-0759-493c-a81d-a58f36f3c77c"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2015-01-01"),
                    FirstName = "Kyle",
                    LastName = "McMaster",
                    Phone = "0123456789",
                    Status = "ACTIVE",
                    Title = "Director of Research"
                },
                new Employee
                {
                    Id = new Guid("ffe20bf7-0b89-497a-a488-675d4cbac05c"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2018-09-25"),
                    FirstName = "Justin",
                    LastName = "Conklin",
                    Phone = "2345678901",
                    Status = "ACTIVE",
                    Title = "Director of User Experience - Dog Division"
                },
                new Employee
                {
                    Id = new Guid("b514189e-f64d-450f-ab9b-14fd6e03273b"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2016-01-25"),
                    FirstName = "Jonathan",
                    LastName = "Keppler",
                    Phone = "3456789012",
                    Status = "ACTIVE",
                    Title = "Digital Storage Analyst"
                }
            };
    }
}
