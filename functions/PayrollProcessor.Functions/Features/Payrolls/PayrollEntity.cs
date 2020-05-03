using System;
using Microsoft.WindowsAzure.Storage.Table;
using PayrollProcessor.Functions.Domain.Features.Employees;
using PayrollProcessor.Functions.Domain.Features.Payrolls;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    /// <summary>
    /// The Table storage representation of a Payroll
    /// </summary>
    public class PayrollEntity : TableEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";

        public static class Map
        {
            public static Payroll To(PayrollEntity entity) =>
                new Payroll(Guid.Parse(entity.RowKey))
                {
                    CheckDate = entity.CheckDate,
                    EmployeeId = entity.EmployeeId,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    EmployeeDepartment = entity.EmployeeDepartment,
                    Version = entity.ETag
                };

            public static PayrollEntity From(Payroll payroll) =>
                new PayrollEntity
                {
                    PartitionKey = payroll.CheckDate.ToString("yyyyMMdd"),
                    RowKey = payroll.Id.ToString("n"),
                    CheckDate = payroll.CheckDate,
                    EmployeeId = payroll.EmployeeId,
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    EmployeeDepartment = payroll.EmployeeDepartment,
                    ETag = payroll.Version
                };

            public static PayrollEntity From(PayrollNew payroll) =>
                new PayrollEntity
                {
                    PartitionKey = payroll.CheckDate.ToString("yyyyMMdd"),
                    RowKey = Guid.NewGuid().ToString("n"),
                    CheckDate = payroll.CheckDate,
                    EmployeeId = payroll.EmployeeId,
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    EmployeeDepartment = payroll.EmployeeDepartment,
                };
        }
    }

    public class EmployeePayrollEntity : TableEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";

        public static class Map
        {
            public static Payroll To(EmployeePayrollEntity entity) =>
                new Payroll(Guid.Parse(entity.RowKey))
                {
                    CheckDate = entity.CheckDate,
                    EmployeeId = entity.EmployeeId,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    EmployeeDepartment = entity.EmployeeDepartment,
                    Version = entity.ETag
                };

            public static EmployeePayrollEntity From(Payroll payroll) =>
                new EmployeePayrollEntity
                {
                    PartitionKey = payroll.EmployeeId.ToString("n"),
                    RowKey = payroll.Id.ToString("n"),
                    CheckDate = payroll.CheckDate,
                    EmployeeId = payroll.EmployeeId,
                    GrossPayroll = payroll.GrossPayroll,
                    PayrollPeriod = payroll.PayrollPeriod,
                    EmployeeDepartment = payroll.EmployeeDepartment,
                    ETag = payroll.Version
                };

            public static Func<EmployeePayroll, EmployeePayrollEntity> From
                (Employee employee) =>
                (EmployeePayroll employeePayroll) =>
                    new EmployeePayrollEntity
                    {
                        PartitionKey = employee.Id.ToString("n"),
                        RowKey = employeePayroll.Id.ToString("n"),
                        CheckDate = employeePayroll.CheckDate,
                        EmployeeId = employee.Id,
                        GrossPayroll = employeePayroll.GrossPayroll,
                        PayrollPeriod = employeePayroll.PayrollPeriod,
                        EmployeeDepartment = employee.Department,
                    };
        }
    }
}
