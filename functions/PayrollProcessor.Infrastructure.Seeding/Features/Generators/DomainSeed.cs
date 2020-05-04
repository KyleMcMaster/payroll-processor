using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Payrolls;
using PayrollProcessor.Infrastructure.Seeding.Features.Generators;

namespace PayrollProcessor.Infrastructure.Seeding.Features.Employees
{
    public class DomainSeed
    {
        private readonly EmployeeSeed employeeSeed;

        public DomainSeed(EmployeeSeed employeeSeed) =>
            this.employeeSeed = employeeSeed;

        public IEnumerable<EmployeeDetails> BuildAll(int employeesCount, int payrollsMaxCount) =>
            employeeSeed.BuildMany(employeesCount, payrollsMaxCount);

        private IEnumerable<Payroll> Build(EmployeeDetails employee, IEnumerable<EmployeePayroll> payrolls) =>
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
