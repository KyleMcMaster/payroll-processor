using System;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Departments
{
    /// <summary>
    /// The Table storage representation of a Department Payroll
    /// </summary>
    public class DepartmentPayrollEntity : CosmosDBEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid EmployeePayrollId { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";
        public string EmployeeFirstName { get; set; } = "";
        public string EmployeeLastName { get; set; } = "";

        public static class Map
        {
            public static DepartmentPayroll ToDepartmentPayroll(DepartmentPayrollEntity entity) =>
                new DepartmentPayroll(entity.Id)
                {
                    CheckDate = entity.CheckDate,
                    EmployeeId = entity.EmployeeId,
                    EmployeePayrollId = entity.EmployeePayrollId,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    EmployeeDepartment = entity.EmployeeDepartment,
                    EmployeeFirstName = entity.EmployeeFirstName,
                    EmployeeLastName = entity.EmployeeLastName,
                    Version = entity.ETag
                };

            public static DepartmentPayrollEntity CreateNewFrom(Employee employee, EmployeePayroll employeePayroll) =>
                new DepartmentPayrollEntity
                {
                    Id = Guid.NewGuid(),
                    PartitionKey = employee.Department.ToLowerInvariant(),
                    EmployeeDepartment = employee.Department,
                    CheckDate = employeePayroll.CheckDate,
                    Type = nameof(DepartmentPayrollEntity),
                    EmployeeId = employeePayroll.EmployeeId,
                    EmployeePayrollId = employeePayroll.Id,
                    GrossPayroll = employeePayroll.GrossPayroll,
                    EmployeeFirstName = employee.FirstName,
                    EmployeeLastName = employee.LastName,
                    PayrollPeriod = employeePayroll.PayrollPeriod
                };
        }
    }
}
