using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using PayrollProcessor.Data.Persistence.Features.Employees;
using Xunit;

namespace PayrollProcessor.Data.Persistence.Tests.Features.Employees;

public class EmployeeRecordMapTests
{
    private readonly EmployeeRecord employeeRecord;
    private readonly IEnumerable<EmployeePayrollRecord> employeePayrolls;

    public EmployeeRecordMapTests()
    {
        var fixture = new Fixture();

        employeeRecord = fixture.Create<EmployeeRecord>();

        employeePayrolls = fixture
            .Build<EmployeePayrollRecord>()
            .With(epr => epr.PartitionKey, employeeRecord.Id.ToString())
            .With(epr => epr.PartitionKey, Guid.NewGuid().ToString())
            .CreateMany();
    }

    [Fact]
    public void ToEmployeeMapsValues()
    {
        var employee = EmployeeRecord.Map.ToEmployee(employeeRecord);

        employee.Id.Should().Be(employeeRecord.Id);
        employee.Department.Should().Be(employeeRecord.Department);
        employee.Email.Should().Be(employeeRecord.Email);
        employee.EmploymentStartedOn.Should().Be(employeeRecord.EmploymentStartedOn);
        employee.FirstName.Should().Be(employeeRecord.FirstName);
        employee.LastName.Should().Be(employeeRecord.LastName);
        employee.Phone.Should().Be(employeeRecord.Phone);
        employee.Status.Should().Be(employeeRecord.Status);
        employee.Title.Should().Be(employeeRecord.Title);
    }

    [Fact]
    public void ToEmployeeDetailsMapsValues()
    {
        var employee = EmployeeRecord.Map.ToEmployeeDetails(employeeRecord, employeePayrolls);

        employee.Id.Should().Be(employeeRecord.Id);
        employee.Department.Should().Be(employeeRecord.Department);
        employee.Email.Should().Be(employeeRecord.Email);
        employee.EmploymentStartedOn.Should().Be(employeeRecord.EmploymentStartedOn);
        employee.FirstName.Should().Be(employeeRecord.FirstName);
        employee.LastName.Should().Be(employeeRecord.LastName);
        employee.Phone.Should().Be(employeeRecord.Phone);
        employee.Status.Should().Be(employeeRecord.Status);
        employee.Title.Should().Be(employeeRecord.Title);

        employee.Payrolls.Should().HaveCount(employeePayrolls.Count());

        var employeePayroll = employee.Payrolls.First();
        employeePayroll.Id.Should().Be(employeePayrolls.First().Id);
        employeePayroll.CheckDate.Should().Be(employeePayrolls.First().CheckDate);
        employeePayroll.EmployeeId.Should().Be(Guid.Parse(employeePayrolls.First().PartitionKey));
        employeePayroll.GrossPayroll.Should().Be(employeePayrolls.First().GrossPayroll);
        employeePayroll.PayrollPeriod.Should().Be(employeePayrolls.First().PayrollPeriod);
    }
}
