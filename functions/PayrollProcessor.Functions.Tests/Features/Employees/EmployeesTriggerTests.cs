using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.WindowsAzure.Storage.Table;
using NSubstitute;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Functions.Features.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PayrollProcessor.Functions.Tests.Features.Employees
{
    public class EmployeesTriggerTests
    {
        private readonly HttpRequest httpRequest;
        private readonly ILogger logger;

        private readonly IEmployeesQueryHandler employeesQueryHandler;
        private readonly IEmployeeCreateCommandHandler employeeCreateCommandHandler;
        private readonly IEmployeeUpdateCommandHandler employeeUpdateCommandHandler;
        private readonly IEmployeePayrollCreateCommandHandler employeePayrollCreateCommandHandler;

        public EmployeesTriggerTests()
        {
            httpRequest = Substitute.For<HttpRequest>();
            logger = Substitute.For<ILogger>();
            employeesQueryHandler = Substitute.For<IEmployeesQueryHandler>();
            employeeCreateCommandHandler = Substitute.For<IEmployeeCreateCommandHandler>();
            employeeUpdateCommandHandler = Substitute.For<IEmployeeUpdateCommandHandler>();
            employeePayrollCreateCommandHandler = Substitute.For<IEmployeePayrollCreateCommandHandler>();
        }

        [Fact]
        public async void GetEmployees_Should_Return_All_Employees()
        {
            var sut = new EmployeesTrigger(
                employeesQueryHandler,
                employeeCreateCommandHandler,
                employeeUpdateCommandHandler,
                employeePayrollCreateCommandHandler);

            var queryValues = new Dictionary<string, StringValues>
            {
                { "count", "4" },
                { "firstName", "first" },
                { "lastName", "last" },
                { "email", "test@test.com" }
            };

            httpRequest.Query.Returns(new QueryCollection(queryValues));

            var employee = new Employee(Guid.NewGuid())
            {
                Email = "test@test.com",
                FirstName = "first",
                LastName = "last",
            };

            employeesQueryHandler
                .GetMany(Arg.Is(4), Arg.Is("first"), Arg.Is("last"), Arg.Is("test@test.com"))
                .Returns(new[] { employee });

            var result = await sut.GetEmployees(httpRequest, logger);

            var employeesResult = result.Value;

            employeesResult.Should().HaveCount(1);
            employeesResult.First().Id.Should().Be(employee.Id);
        }
    }
}
