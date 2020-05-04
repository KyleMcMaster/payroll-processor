using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bogus;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Payrolls;
using PayrollProcessor.Infrastructure.Seeding.Infrastructure;

namespace PayrollProcessor.Infrastructure.Seeding.Features.Generators
{
    public class PayrollSeed
    {
        private readonly Faker<Payroll> payrolls;

        public PayrollSeed() =>
            payrolls = new DomainFaker<Payroll>(faker => new Payroll(Guid.NewGuid()))
                    .RuleFor(e => e.Id, f => Guid.NewGuid())
                    .RuleFor(e => e.EmployeeId, f => Guid.NewGuid())
                    .RuleFor(e => e.GrossPayroll, f => f.Finance.Amount(300, 2_500))
                    .RuleFor(e => e.PayrollPeriod, (f, e) => (ISOWeek.GetWeekOfYear(e.CheckDate.DateTime) / 2).ToString().PadLeft(2, '0'))
                    .RuleFor(e => e.CheckDate, f => f.Date.Past())
                    .RuleFor(e => e.EmployeeDepartment, f => f.PickRandom(EmployeeDepartment.All.Select(s => s.CodeName).ToList()));

        public IEnumerable<Payroll> BuildMany(int count) =>
            payrolls.Generate(count);
    }
}
