using System;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Data.Persistence.Infrastructure.Records;

namespace PayrollProcessor.Data.Persistence.Features.Departments;

public class DepartmentEmployeeRecord : CosmosDBRecord
{
    public Guid EmployeeId { get; set; }
    public string Department { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public DepartmentEmployeeRecord() => Type = nameof(DepartmentEmployeeRecord);

    public static class Map
    {
        public static DepartmentEmployee ToDepartmentEmployee(DepartmentEmployeeRecord entity) =>
            new DepartmentEmployee(entity.Id)
            {
                Email = entity.Email,
                EmployeeId = entity.EmployeeId,
                Department = entity.Department,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Version = entity.ETag
            };

        public static DepartmentEmployeeRecord From(DepartmentEmployee employee) =>
            new DepartmentEmployeeRecord
            {
                Id = employee.Id,
                PartitionKey = employee.Department.ToLowerInvariant(),
                Department = employee.Department,
                Email = employee.Email,
                EmployeeId = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                ETag = employee.Version
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
            };

        public static DepartmentEmployeeRecord Merge(Employee employee, DepartmentEmployee departmentEmployee)
        {
            var recordToUpdate = From(departmentEmployee);

            recordToUpdate.Email = employee.Email;
            recordToUpdate.FirstName = employee.FirstName;
            recordToUpdate.LastName = employee.LastName;

            return recordToUpdate;
        }
    }
}
