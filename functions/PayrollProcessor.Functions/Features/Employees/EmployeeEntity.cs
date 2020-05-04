using System;
using System.Collections.Generic;
using System.Linq;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Payrolls;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Employees
{
    /// <summary>
    /// The datastore representation of an employee
    /// </summary>
    public class EmployeeEntity : CosmosDBEntity
    {
        public string EmployeeId { get; set; } = "";
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
                new Employee(Guid.Parse(entity.Id))
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
                new EmployeeDetails(Guid.Parse(entity.Id))
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
                string partitionId = employee.Id.ToString("n");

                return new EmployeeEntity
                {
                    Id = partitionId,
                    EmployeeId = partitionId,
                    Type = nameof(Employee),
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

            public static EmployeeEntity From(EmployeeNew employee) =>
                new EmployeeEntity
                {
                    Id = Guid.NewGuid().ToString("n"),
                    Type = nameof(Employee),
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

    public class EmployeePayrollEntity : CosmosDBEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public string EmployeeId { get; set; } = "";
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";

        public static class Map
        {
            public static EmployeePayroll ToEmployeePayroll(EmployeePayrollEntity entity) =>
                new EmployeePayroll(Guid.Parse(entity.Id))
                {
                    EmployeeId = Guid.Parse(entity.EmployeeId),
                    CheckDate = entity.CheckDate,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    Version = entity.ETag,
                };

            public static EmployeePayrollEntity From(EmployeePayroll payroll) =>
                new EmployeePayrollEntity
                {
                    Id = payroll.Id.ToString("n"),
                    Type = nameof(Payroll),
                    CheckDate = payroll.CheckDate,
                    EmployeeId = payroll.EmployeeId.ToString("n"),
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    ETag = payroll.Version
                };

            public static EmployeePayrollEntity From(Payroll payroll) =>
                new EmployeePayrollEntity
                {
                    Id = payroll.Id.ToString("n"),
                    Type = nameof(Payroll),
                    CheckDate = payroll.CheckDate,
                    EmployeeId = payroll.EmployeeId.ToString("n"),
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    ETag = payroll.Version
                };

            public static EmployeePayrollEntity From(EmployeePayrollNew payroll) =>
                new EmployeePayrollEntity
                {
                    Id = Guid.NewGuid().ToString("n"),
                    Type = nameof(Payroll),
                    CheckDate = payroll.CheckDate,
                    EmployeeId = payroll.EmployeeId.ToString("n"),
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                };
        }
    }
}
