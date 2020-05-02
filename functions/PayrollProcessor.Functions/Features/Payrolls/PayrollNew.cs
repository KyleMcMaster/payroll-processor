using System;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public class PayrollNew
    {
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
        public string EmployeeDepartment { get; set; } = "";
    }
}
