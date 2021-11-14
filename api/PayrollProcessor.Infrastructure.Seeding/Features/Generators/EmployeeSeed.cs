using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Bogus;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Infrastructure.Seeding.Infrastructure;

namespace PayrollProcessor.Infrastructure.Seeding.Features.Generators;

public class EmployeeSeed
{
    private readonly Faker<EmployeeDetail> employeeGenerator;
    private readonly Faker<EmployeePayroll> employeePayrollGenerator;
    private readonly Faker faker;

    public EmployeeSeed()
    {
        faker = new Faker();

        string[] statusNames = new[] { EmployeeStatus.Enabled.CodeName, EmployeeStatus.Disabled.CodeName };
        float[] statusWeights = new[] { .98f, .02f };

        employeeGenerator = new DomainFaker<EmployeeDetail>(faker => new EmployeeDetail(Guid.NewGuid()))
            .RuleFor(e => e.Department, f => f.PickRandom(EmployeeDepartment.All.Select(s => s.CodeName).ToList()))
            .RuleFor(e => e.FirstName, f => f.Name.FirstName())
            .RuleFor(e => e.LastName, f => f.Name.LastName())
            .RuleFor(e => e.EmploymentStartedOn, f => f.Date.PastOffset(10))
            .RuleFor(e => e.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(e => e.Title, f => f.Name.JobTitle())
            .RuleFor(e => e.Status, f => f.Random.WeightedRandom(statusNames, statusWeights))
            .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName));

        employeePayrollGenerator = new DomainFaker<EmployeePayroll>(faker => new EmployeePayroll(Guid.NewGuid()))
            .RuleFor(e => e.CheckDate, f => f.Date.Past())
            .RuleFor(e => e.GrossPayroll, f => f.Finance.Amount(300, 2_500))
            .RuleFor(e => e.PayrollPeriod, (f, e) => (ISOWeek.GetWeekOfYear(e.CheckDate.DateTime) / 2).ToString().PadLeft(2, '0'));
    }

    public IEnumerable<EmployeeDetail> BuildMany(int employeesCount, int payrollsMaxCount) =>
        employeeGenerator
            .Generate(employeesCount)
            .Select(e =>
            {
                e.Payrolls = employeePayrollGenerator.Generate(faker.Random.Int(1, payrollsMaxCount));

                foreach (var payroll in e.Payrolls)
                {
                    payroll.EmployeeId = e.Id;
                }

                return e;
            });

}
