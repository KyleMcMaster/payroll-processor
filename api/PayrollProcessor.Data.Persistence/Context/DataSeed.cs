using PayrollProcessor.Data.Persistence.Features.Employees;
using PayrollProcessor.Data.Persistence.Features.Risks;
using System;
using System.Linq;

namespace PayrollProcessor.Data.Persistence.Context
{
    public static class DataSeed
    {
        public static IQueryable<RiskRecord> Risks()
        {
            var risks = new RiskRecord[]
            {
                new RiskRecord
                {
                    Id = new Guid("429c49ae-4728-48de-927a-68c50c7c80b1"),
                    CodeName = "LOW",
                    DisplayName = "Low"
                },
                new RiskRecord
                {
                    Id = new Guid("50b7eac5-0759-493c-a81d-a58f36f3c77c"),
                    CodeName = "MEDIUM",
                    DisplayName = "Medium"
                },
                new RiskRecord
                {
                    Id = new Guid("ffe20bf7-0b89-497a-a488-675d4cbac05c"),
                    CodeName = "HIGH",
                    DisplayName = "High"
                }
            };

            return risks.AsQueryable();
        }

        public static int EmployeeCount => Employees().Count();

        public static IQueryable<EmployeeRecord> Employees() =>
            new EmployeeRecord[]
            {
                new EmployeeRecord
                {
                    Id = new Guid("429c49ae-4728-48de-927a-68c50c7c80b1"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2015-01-01"),
                    FirstName = "Nathan",
                    LastName = "Harper",
                    Phone = "1234567890",
                    Title = "CIO"
                },
                new EmployeeRecord
                {
                    Id = new Guid("50b7eac5-0759-493c-a81d-a58f36f3c77c"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2015-01-01"),
                    FirstName = "Kyle",
                    LastName = "McMaster",
                    Phone = "0123456789",
                    Title = "Director of Research"
                },
                new EmployeeRecord
                {
                    Id = new Guid("ffe20bf7-0b89-497a-a488-675d4cbac05c"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2018-09-25"),
                    FirstName = "Justin",
                    LastName = "Conklin",
                    Phone = "2345678901",
                    Title = "Director of User Experience - Dog Division"
                },
                new EmployeeRecord
                {
                    Id = new Guid("b514189e-f64d-450f-ab9b-14fd6e03273b"),
                    Department = "IT",
                    EmploymentStartedOn = DateTimeOffset.Parse("2016-01-25"),
                    FirstName = "Jonathan",
                    LastName = "Keppler",
                    Phone = "3456789012",
                    Title = "Digital Storage Analyst"
                }
            }.AsQueryable();
    }
}
