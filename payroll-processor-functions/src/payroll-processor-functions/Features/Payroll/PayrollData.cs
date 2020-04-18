using System;
using System.Collections.Generic;

namespace PayrollProcessor.Functions.Features.Payroll
{
    public class PayrollData
    {
        public static IEnumerable<Payroll> Payrolls() =>
            new Payroll[]
            {
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow,
                    EmployeeId = new Guid("ffe20bf7-0b89-497a-a488-675d4cbac05c"),
                    GrossPayroll = 1000.00,
                    PayrollPeriod = "2"
                },
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow,
                    EmployeeId = new Guid("b514189e-f64d-450f-ab9b-14fd6e03273b"),
                    GrossPayroll = 1000.00,
                    PayrollPeriod = "2"
                },
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow,
                    EmployeeId = new Guid("429c49ae-4728-48de-927a-68c50c7c80b1"),
                    GrossPayroll = 2000.00,
                    PayrollPeriod = "2"
                },
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow,
                    EmployeeId = new Guid("50b7eac5-0759-493c-a81d-a58f36f3c77c"),
                    GrossPayroll = 2000.00,
                    PayrollPeriod = "2"
                },
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow,
                    EmployeeId = new Guid("d24ac3b8-edb2-47c7-9599-641530800c2b"),
                    GrossPayroll = 500,
                    PayrollPeriod = "2"
                },
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow.AddDays(-14),
                    EmployeeId = new Guid("429c49ae-4728-48de-927a-68c50c7c80b1"),
                    GrossPayroll = 1000.00,
                    PayrollPeriod = "1"
                },
                new Payroll
                {
                    Id = Guid.NewGuid(),
                    CheckDate = DateTimeOffset.UtcNow.AddDays(-14),
                    EmployeeId = new Guid("50b7eac5-0759-493c-a81d-a58f36f3c77c"),
                    GrossPayroll = 1000.00,
                    PayrollPeriod = "1"
                },
            };
    }
}
