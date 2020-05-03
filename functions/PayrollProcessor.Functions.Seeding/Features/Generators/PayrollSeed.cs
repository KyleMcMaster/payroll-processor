using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using PayrollProcessor.Functions.Domain.Features.Employees;
using PayrollProcessor.Functions.Domain.Features.Payrolls;
using PayrollProcessor.Functions.Seeding.Infrastructure;

namespace PayrollProcessor.Functions.Seeding.Features.Generators
{
    public class PayrollSeed
    {
        private readonly Faker<Payroll> payrolls;

        public PayrollSeed() =>
            payrolls = new DomainFaker<Payroll>(typeof(Payroll).GetConstructors().First(), () => new Payroll(Guid.NewGuid()))
                    .RuleFor(e => e.Id, f => Guid.NewGuid())
                    .RuleFor(e => e.EmployeeId, f => Guid.NewGuid())
                    .RuleFor(e => e.GrossPayroll, f => f.Finance.Amount(300, 2_500))
                    .RuleFor(e => e.PayrollPeriod, f => f.Random.Int(1, 26).ToString())
                    .RuleFor(e => e.CheckDate, f => f.Date.Past())
                    .RuleFor(e => e.EmployeeDepartment, f => f.PickRandom(EmployeeDepartment.All.Select(s => s.CodeName).ToList()));

        public IEnumerable<Payroll> BuildMany(int count) =>
            payrolls.Generate(count);
    }
}
