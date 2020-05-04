using System;
using PayrollProcessor.Core.Domain.Features.Employees;
using PayrollProcessor.Core.Domain.Features.Payrolls;
using PayrollProcessor.Functions.Infrastructure;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    /// <summary>
    /// The Table storage representation of a Payroll
    /// </summary>
    public class PayrollEntity : CosmosDBEntity
    {
        public string CheckDate { get; set; } = "";
        public string EmployeeId { get; set; } = "";
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";
        public string EmployeeFirstName { get; set; } = "";
        public string EmployeeLastName { get; set; } = "";

        public static class Map
        {
            public static Payroll ToPayroll(PayrollEntity entity) =>
                new Payroll(Guid.Parse(entity.Id))
                {
                    CheckDate = DateTimeOffset.Parse(entity.CheckDate),
                    EmployeeId = Guid.Parse(entity.EmployeeId),
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    EmployeeDepartment = entity.EmployeeDepartment,
                    EmployeeFirstName = entity.EmployeeFirstName,
                    EmployeeLastName = entity.EmployeeLastName,
                    Version = entity.ETag
                };

            public static PayrollEntity From(Payroll payroll) =>
                new PayrollEntity
                {
                    Id = payroll.Id.ToString("n"),
                    Type = nameof(Payroll),
                    CheckDate = payroll.CheckDate.ToString("yyyyMMdd"),
                    EmployeeId = payroll.EmployeeId.ToString("n"),
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    EmployeeDepartment = payroll.EmployeeDepartment,
                    EmployeeFirstName = payroll.EmployeeFirstName,
                    EmployeeLastName = payroll.EmployeeLastName,
                    ETag = payroll.Version
                };

            public static PayrollEntity From(Employee employee, EmployeePayroll payroll) =>
                new PayrollEntity
                {
                    Id = Guid.NewGuid().ToString("n"),
                    Type = nameof(Payroll),
                    CheckDate = payroll.CheckDate.ToString("yyyyMMdd"),
                    EmployeeId = payroll.EmployeeId.ToString("n"),
                    GrossPayroll = payroll.GrossPayroll,
                    EmployeeDepartment = employee.Department,
                    EmployeeFirstName = employee.FirstName,
                    EmployeeLastName = employee.LastName,
                    PayrollPeriod = payroll.PayrollPeriod
                };
        }
    }
}
