using System;
using AutoFixture.Idioms;
using FluentAssertions;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Tests.Fixtures;
using Xunit;

namespace PayrollProcessor.Core.Domain.Tests.Features.Employees;

public class EmployeePayrollCreateCommandTests
{
    [Fact]
    public void ConstructorGuardsAgainstInvalidParameters()
    {
        var assertion = new GuardClauseAssertion(new DomainFixture());

        assertion.Verify(typeof(EmployeePayrollCreateCommand).GetConstructors());
    }

    [Theory, AutoDomainData]
    public void ConstructorAssignsPropertiesFromParameters(Guid newPayrollId, Employee employee, EmployeePayrollNew employeePayroll)
    {
        var sut = new EmployeePayrollCreateCommand(employee, newPayrollId, employeePayroll);

        sut.Employee.Should().Be(employee);
        sut.NewPayrollId.Should().Be(newPayrollId);
        sut.NewPayroll.Should().Be(employeePayroll);
    }
}
