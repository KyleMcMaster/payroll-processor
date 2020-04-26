using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public class PayrollEntity : TableEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public double GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";

        public static class Map
        {
            public static Payroll To(PayrollEntity entity)
            {
                return new Payroll(Guid.Parse(entity.RowKey))
                {
                    CheckDate = entity.CheckDate,
                    EmployeeId = entity.EmployeeId,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    EmployeeDepartment = entity.EmployeeDepartment,
                    Version = entity.ETag
                };
            }

            public static PayrollEntity From(Payroll payroll)
            {
                return new PayrollEntity
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
            }
        }
    }

    public class EmployeePayrollEntity : TableEntity
    {
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public double GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";

        public static class Map
        {
            public static Payroll To(EmployeePayrollEntity entity)
            {
                return new Payroll(Guid.Parse(entity.RowKey))
                {
                    CheckDate = entity.CheckDate,
                    EmployeeId = entity.EmployeeId,
                    GrossPayroll = entity.GrossPayroll,
                    PayrollPeriod = entity.PayrollPeriod,
                    EmployeeDepartment = entity.EmployeeDepartment,
                    Version = entity.ETag
                };
            }

            public static EmployeePayrollEntity From(Payroll payroll)
            {
                return new EmployeePayrollEntity
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
            }
        }
    }
}
