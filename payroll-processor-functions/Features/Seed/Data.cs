using System;
using System.Collections.Generic;

namespace PayrollProcessor.Functions.Features.Seed
{
    public static class Data
    {
        public static IEnumerable<Employee> GetEmployees() =>
            new Employee[]
            {
                new Employee
                {
                    Id = new Guid("429c49ae-4728-48de-927a-68c50c7c80b1"),
                    EmploymentStartedOn = DateTimeOffset.Parse("2016-01-02"),
                    FirstName = "Nathan",
                    LastName = "Harper",
                    Phone = "123456789",
                    Title = "CIO"
                },
                new Employee
                {
                    Id = new Guid("ffe20bf7-0b89-497a-a488-675d4cbac05c"),
                    EmploymentStartedOn = DateTimeOffset.Parse("2016-01-02"),
                    FirstName = "Kyle",
                    LastName = "McMaster",
                    Phone = "012345678",
                    Title = "Director of Research"
                }
            };
    }
}
