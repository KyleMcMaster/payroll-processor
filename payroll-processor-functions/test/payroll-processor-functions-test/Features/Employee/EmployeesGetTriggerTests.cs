using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;
using PayrollProcessor.Functions.Features.Employee;
using System.Collections.Generic;
using Xunit;

namespace PayrollProcessor.Functions.Tests.Features.Employee
{
    public class EmployeesGetTriggerTests
    {
        private HttpRequest DefaultHttpRequest { get; }
        private ILogger Logger { get; }
        private EmployeesGetTrigger Trigger { get; }

        public EmployeesGetTriggerTests()
        {
            DefaultHttpRequest = Substitute.For<HttpRequest>();

            Logger = Substitute.For<ILogger>();

            Trigger = new EmployeesGetTrigger();
        }

        [Fact]
        public async void Run_Should_Return_Count_Of_All_Employees()
        {
            var result = (OkObjectResult)await Trigger.Run(DefaultHttpRequest, Logger);

            string resultValue = result.Value.ToString();

            var employeesResult = JsonConvert
                .DeserializeObject<IEnumerable<Functions.Features.Employee.Employee>>(resultValue);

            employeesResult.Should().HaveCount(EmployeeData.EmployeeCount);
        }
    }
}
