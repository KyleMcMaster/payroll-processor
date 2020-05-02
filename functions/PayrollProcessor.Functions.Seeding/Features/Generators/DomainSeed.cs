using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using PayrollProcessor.Functions.Domain.Features.Employees;
using PayrollProcessor.Functions.Domain.Features.Payrolls;
using PayrollProcessor.Functions.Seeding.Features.Generators;

namespace PayrollProcessor.Functions.Seeding.Features.Employees
{
    public class DomainSeed
    {
        private readonly EmployeeSeed employeeSeed;

        public DomainSeed(EmployeeSeed employeeSeed) =>
            this.employeeSeed = employeeSeed;

        public IEnumerable<(Employee, IEnumerable<Payroll>)> BuildAll(int employeesCount, int payrollsMaxCount) =>
            employeeSeed
                .BuildMany(employeesCount, payrollsMaxCount)
                .Select(e =>
                {
                    var payrolls = Build(e, e.Payrolls)
                        .ToList()
                        .AsEnumerable();

                    return (e, payrolls);
                });

        private IEnumerable<Payroll> Build(Employee employee, IEnumerable<EmployeePayroll> payrolls) =>
            payrolls.Select(p => new Payroll(Guid.NewGuid())
            {
                CheckDate = p.CheckDate,
                EmployeeDepartment = employee.Department,
                EmployeeId = employee.Id,
                GrossPayroll = p.GrossPayroll,
                PayrollPeriod = p.PayrollPeriod
            });
    }
}
