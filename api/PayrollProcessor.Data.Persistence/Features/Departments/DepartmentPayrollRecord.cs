using System;
using PayrollProcessor.Core.Domain.Features.Departments;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Data.Persistence.Infrastructure.Records;

namespace PayrollProcessor.Data.Persistence.Features.Departments
{
    public class DepartmentPayrollRecord : CosmosDBRecord
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
            public static DepartmentPayroll ToDepartmentPayroll(DepartmentPayrollRecord entity) =>
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

            public static DepartmentPayrollRecord CreateNewFrom(Employee employee, Guid recordId, EmployeePayroll employeePayroll) =>
                new DepartmentPayrollRecord
                {
                    Id = recordId,
                    PartitionKey = employee.Department.ToLowerInvariant(),
                    EmployeeDepartment = employee.Department,
                    CheckDate = employeePayroll.CheckDate,
                    Type = nameof(DepartmentPayrollRecord),
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
