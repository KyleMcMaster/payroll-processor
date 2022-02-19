using System;
using System.Collections.Generic;
using System.Linq;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Data.Persistence.Infrastructure.Records;

namespace PayrollProcessor.Data.Persistence.Features.Employees;

public class EmployeeRecord : CosmosDBRecord
{
    public string Department { get; set; } = string.Empty;
    public DateTimeOffset EmploymentStartedOn { get; set; }
    public string Email { get; set; } = string.Empty;
    public string EmailLower { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string FirstNameLower { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string LastNameLower { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public EmployeeRecord() => Type = nameof(EmployeeRecord);

    public static class Map
    {
        public static Employee ToEmployee(EmployeeRecord entity) =>
            new Employee(entity.Id)
            {
                Department = entity.Department,
                EmploymentStartedOn = entity.EmploymentStartedOn,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Phone = entity.Phone,
                Email = entity.Email,
                Status = entity.Status,
                Title = entity.Title,
                Version = entity.ETag
            };

        public static EmployeeDetail ToEmployeeDetails(EmployeeRecord entity, IEnumerable<EmployeePayrollRecord> payrolls) =>
            new EmployeeDetail(entity.Id)
            {
                Department = entity.Department,
                EmploymentStartedOn = entity.EmploymentStartedOn,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Phone = entity.Phone,
                Email = entity.Email,
                Status = entity.Status,
                Title = entity.Title,
                Version = entity.ETag,
                Payrolls = payrolls.Select(EmployeePayrollRecord.Map.ToEmployeePayroll)
            };

        public static EmployeeRecord From(Employee employee)
        {
            var partitionId = employee.Id;

            return new EmployeeRecord
            {
                Id = partitionId,
                PartitionKey = partitionId.ToString(),
                Department = employee.Department,
                EmploymentStartedOn = employee.EmploymentStartedOn,
                FirstName = employee.FirstName,
                FirstNameLower = employee.FirstName.ToLowerInvariant(),
                LastName = employee.LastName,
                LastNameLower = employee.LastName.ToLowerInvariant(),
                Email = employee.Email,
                EmailLower = employee.Email.ToLowerInvariant(),
                Phone = employee.Phone,
                Status = employee.Status,
                Title = employee.Title,
            };
        }

        public static EmployeeRecord From(Guid newId, EmployeeNew employee) =>
            new EmployeeRecord
            {
                Id = newId,
                PartitionKey = newId.ToString(),
                Department = employee.Department,
                EmploymentStartedOn = employee.EmploymentStartedOn,
                FirstName = employee.FirstName,
                FirstNameLower = employee.FirstName.ToLowerInvariant(),
                LastName = employee.LastName,
                LastNameLower = employee.LastName.ToLowerInvariant(),
                Email = employee.Email,
                EmailLower = employee.Email.ToLowerInvariant(),
                Phone = employee.Phone,
                Status = employee.Status,
                Title = employee.Title
            };

        public static EmployeeRecord Merge(EmployeeUpdateCommand command)
        {
            var recordToUpdate = From(command.EntityToUpdate);

            recordToUpdate.Email = command.Email;
            recordToUpdate.EmailLower = command.Email.ToLowerInvariant();
            recordToUpdate.EmploymentStartedOn = command.EmploymentStartedOn;
            recordToUpdate.FirstName = command.FirstName;
            recordToUpdate.FirstNameLower = command.FirstName.ToLowerInvariant();
            recordToUpdate.LastName = command.LastName;
            recordToUpdate.LastNameLower = command.LastName.ToLowerInvariant();
            recordToUpdate.Phone = command.Phone;
            recordToUpdate.Status = command.Status;
            recordToUpdate.Title = command.Title;

            return recordToUpdate;
        }
    }
}
