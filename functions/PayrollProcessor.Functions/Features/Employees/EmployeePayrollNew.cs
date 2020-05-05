using System;

namespace PayrollProcessor.Functions.Features.Employees
{
    public class EmployeePayrollNew
    {
        public DateTimeOffset CheckDate { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
    }
}
