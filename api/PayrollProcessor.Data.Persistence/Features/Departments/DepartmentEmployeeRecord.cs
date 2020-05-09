using System;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Data.Persistence.Infrastructure.Records;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentEmployeeRecord : CosmosDBRecord
    {
        public Guid EmployeeId { get; set; }
        public string Department { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public static class Map
        {
            public static DepartmentEmployee ToDepartmentEmployee(DepartmentEmployeeRecord entity) =>
                new DepartmentEmployee(entity.Id)
                {
                    Email = entity.Email,
                    EmployeeId = entity.EmployeeId,
                    Department = entity.Department,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName
                };

            public static DepartmentEmployeeRecord CreateNewFrom(Employee employee, Guid recordId) =>
                new DepartmentEmployeeRecord
                {
                    Id = recordId,
                    PartitionKey = employee.Department.ToLowerInvariant(),
                    Department = employee.Department,
                    Email = employee.Email,
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Type = nameof(DepartmentEmployeeRecord)
                };
        }
    }
}
