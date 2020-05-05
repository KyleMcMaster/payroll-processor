using System;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Departments
{
    public class DepartmentEmployeeEntity : CosmosDBEntity
    {
        public Guid EmployeeId { get; set; }
        public string Department { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public static class Map
        {
            public static DepartmentEmployee ToDepartmentEmployee(DepartmentEmployeeEntity entity) =>
                new DepartmentEmployee(entity.Id)
                {
                    Email = entity.Email,
                    EmployeeId = entity.EmployeeId,
                    Department = entity.Department,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName
                };

            public static DepartmentEmployeeEntity CreateNewFrom(Employee employee) =>
                new DepartmentEmployeeEntity
                {
                    Id = Guid.NewGuid(),
                    PartitionKey = employee.Department.ToLowerInvariant(),
                    Department = employee.Department,
                    Email = employee.Email,
                    EmployeeId = employee.Id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Type = nameof(DepartmentEmployeeEntity)
                };
        }
    }
}
