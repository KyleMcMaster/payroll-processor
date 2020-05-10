using System;
using AutoFixture.Idioms;
using FluentAssertions;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Tests.Fixtures;
using Xunit;

namespace PayrollProcessor.Core.Domain.Tests.Features.Employees
{
    public class EmployeeCreateCommandTests
    {
        [Fact]
        public void Constructor_Guards_Against_Invalid_Parameters()
        {
            var assertion = new GuardClauseAssertion(new DomainFixture());

            assertion.Verify(typeof(EmployeeCreateCommand).GetConstructors());
        }

        [Theory, AutoDomainData]
        public void Constructor_Assigns_Properties_From_Parameters(Guid newId, EmployeeNew employee)
        {
            var sut = new EmployeeCreateCommand(newId, employee);

            sut.NewId.Should().Be(newId);
            sut.Employee.Should().Be(employee);
        }
    }
}
