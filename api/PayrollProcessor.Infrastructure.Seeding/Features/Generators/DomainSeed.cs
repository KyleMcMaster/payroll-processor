using System;
using System.Collections.Generic;
using System.Linq;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Infrastructure.Seeding.Features.Generators;

namespace PayrollProcessor.Infrastructure.Seeding.Features.Employees;

public class DomainSeed
{
    private readonly EmployeeSeed employeeSeed;

    public DomainSeed(EmployeeSeed employeeSeed) =>
        this.employeeSeed = employeeSeed;

    public IEnumerable<EmployeeDetail> BuildAll(int employeesCount, int payrollsMaxCount) =>
        employeeSeed.BuildMany(employeesCount, payrollsMaxCount);
}
