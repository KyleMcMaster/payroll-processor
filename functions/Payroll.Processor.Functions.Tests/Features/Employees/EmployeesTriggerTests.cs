using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using NSubstitute;
using Payroll.Processor.Functions.Features.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Payroll.Processor.Functions.Tests.Features.Employees
{
    public class EmployeesTriggerTests
    {
        private HttpRequest DefaultHttpRequest { get; }
        private CloudTable Table { get; }
        private ILogger Logger { get; }
        private EmployeesTrigger Trigger { get; }

        public EmployeesTriggerTests()
        {
            DefaultHttpRequest = Substitute.For<HttpRequest>();

            Logger = Substitute.For<ILogger>();

            Table = TableStorageFixtures.CreateTable("employees");

            Trigger = new EmployeesTrigger();
        }

        [Fact]
        public async void GetEmployees_Should_Return_All_Employees()
        {
            var entity = new EmployeeEntity { PartitionKey = "employees", RowKey = Guid.NewGuid().ToString("n") };

            var segment = TableStorageFixtures.CreateSegment(new[] { entity });

            Table
                .ExecuteQuerySegmentedAsync(Arg.Any<TableQuery<EmployeeEntity>>(), Arg.Any<TableContinuationToken>())
                .Returns(segment);

            var result = await Trigger.GetEmployees(DefaultHttpRequest, Table, Logger);

            var employeesResult = result.Value;

            employeesResult.Should().HaveCount(1);
            employeesResult.First().Id.Should().Be(Guid.Parse(entity.RowKey));
        }
    }

    public static class TableStorageFixtures
    {
        public static CloudTable CreateTable(string tableName) =>
            Substitute.For<CloudTable>(new Uri($"http://127.0.0.1:10002/devstoreaccount1/{tableName}"));

        public static TableQuerySegment<T> CreateSegment<T>(IEnumerable<T> data)
        {
            // https://github.com/Azure/azure-storage-net/issues/619#issuecomment-364090291

            var ctor = typeof(TableQuerySegment<T>)
                .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                .FirstOrDefault(c => c.GetParameters().Count() == 1);

            return ctor.Invoke(new object[] { data.ToList() }) as TableQuerySegment<T>;
        }
    }
}
