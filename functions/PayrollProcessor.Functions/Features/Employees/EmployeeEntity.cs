using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Employees
{
    /// <summary>
    /// The table storage representation of an employee
    /// </summary>
    public class EmployeeEntity : TableEntity
    {
        public string Department { get; set; } = "";
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Title { get; set; } = "";
        public string Payrolls { get; set; } = "";

        public static class Map
        {
            public static Employee ToEmployee(EmployeeEntity entity)
            {
                var payrolls = JsonConvert.DeserializeObject<IEnumerable<EmployeePayroll>>(entity.Payrolls);

                return new Employee(Guid.Parse(entity.RowKey))
                {
                    Department = entity.Department,
                    EmploymentStartedOn = entity.EmploymentStartedOn,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Phone = entity.Phone,
                    Status = entity.Status,
                    Title = entity.Title,
                    Payrolls = payrolls,
                    Version = entity.ETag
                };
            }

            public static EmployeeEntity From(Employee employee)
            {
                string payrolls = JsonConvert.SerializeObject(employee.Payrolls, DefaultJsonSerializerSettings.JsonSerializerSettings);

                return new EmployeeEntity
                {
                    PartitionKey = employee.Department.ToLowerInvariant(),
                    RowKey = employee.Id.ToString("n"),
                    Department = employee.Department,
                    EmploymentStartedOn = employee.EmploymentStartedOn,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Status = employee.Status,
                    Title = employee.Title,
                    Payrolls = payrolls,
                    ETag = employee.Version
                };
            }

            public static EmployeeEntity From(EmployeeNew employee) =>
                new EmployeeEntity
                {
                    PartitionKey = employee.Department.ToLowerInvariant(),
                    RowKey = Guid.NewGuid().ToString("n"),
                    Department = employee.Department,
                    EmploymentStartedOn = employee.EmploymentStartedOn,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Status = employee.Status,
                    Title = employee.Title,
                    Payrolls = "",
                };
        }
    }
}
