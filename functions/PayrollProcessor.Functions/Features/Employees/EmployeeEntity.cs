using System;
using System.Collections.Generic;
using System.Linq;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Employees
{
    /// <summary>
    /// The datastore representation of an employee
    /// </summary>
    public class EmployeeEntity : CosmosDBEntity
    {
        public string Department { get; set; } = "";
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string Email { get; set; } = "";
        public string EmailLower { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string FirstNameLower { get; set; } = "";
        public string LastName { get; set; } = "";
        public string LastNameLower { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Title { get; set; } = "";

        public static class Map
        {
            public static Employee ToEmployee(EmployeeEntity entity) =>
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

            public static EmployeeDetails ToEmployeeDetails(EmployeeEntity entity, IEnumerable<EmployeePayrollEntity> payrolls) =>
                new EmployeeDetails(entity.Id)
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
                    Payrolls = payrolls.Select(EmployeePayrollEntity.Map.ToEmployeePayroll)
                };

            public static EmployeeEntity From(Employee employee)
            {
                var partitionId = employee.Id;

                return new EmployeeEntity
                {
                    Id = partitionId,
                    PartitionKey = partitionId.ToString(),
                    Type = nameof(EmployeeEntity),
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

            public static EmployeeEntity From(EmployeeNew employee)
            {
                var partitionId = Guid.NewGuid();

                return new EmployeeEntity
                {
                    Id = partitionId,
                    PartitionKey = partitionId.ToString(),
                    Type = nameof(EmployeeEntity),
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
            }
        }
    }

    /// <summary>
    /// The datastore representation of a single payroll record for a given employee
    /// </summary>
    public class EmployeePayrollEntity : CosmosDBEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";

        public static class Map
        {
            public static EmployeePayroll ToEmployeePayroll(EmployeePayrollEntity entity) =>
                new EmployeePayroll(entity.Id)
                {
                    EmployeeId = Guid.Parse(entity.PartitionKey),
                    CheckDate = entity.CheckDate,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    Version = entity.ETag,
                };

            public static EmployeePayrollEntity From(EmployeePayroll payroll) =>
                new EmployeePayrollEntity
                {
                    Id = payroll.Id,
                    PartitionKey = payroll.EmployeeId.ToString(),
                    Type = nameof(EmployeePayrollEntity),
                    CheckDate = payroll.CheckDate,
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    ETag = payroll.Version
                };

            public static EmployeePayrollEntity From(Employee employee, EmployeePayrollNew payroll) =>
                new EmployeePayrollEntity
                {
                    Id = Guid.NewGuid(),
                    PartitionKey = employee.Id.ToString(),
                    Type = nameof(EmployeePayrollEntity),
                    CheckDate = payroll.CheckDate,
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                };
        }
    }
}
