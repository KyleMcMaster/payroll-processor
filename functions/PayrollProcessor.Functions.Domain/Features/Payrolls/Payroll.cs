using System;

namespace PayrollProcessor.Functions.Domain.Features.Payrolls
{
    public class Payroll
    {
        public Guid Id { get; set; }
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";
        public string Version { get; set; } = "";

        public Payroll(Guid id) => Id = id;
    }
}