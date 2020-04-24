using System;
using Microsoft.WindowsAzure.Storage.Table;

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

        public static class Map
        {
            public static Employee To(EmployeeEntity entity)
            {
                return new Employee(Guid.Parse(entity.RowKey))
                {
                    Department = entity.Department,
                    EmploymentStartedOn = entity.EmploymentStartedOn,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Phone = entity.Phone,
                    Status = entity.Status,
                    Title = entity.Status
                };
            }

            public static EmployeeEntity From(Employee employee)
            {
                return new EmployeeEntity
                {
                    PartitionKey = "Employee",
                    RowKey = employee.Id.ToString("n"),
                    Department = employee.Department,
                    EmploymentStartedOn = employee.EmploymentStartedOn,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Phone = employee.Phone,
                    Status = employee.Status,
                    Title = employee.Title
                };
            }
        }
    }
}
