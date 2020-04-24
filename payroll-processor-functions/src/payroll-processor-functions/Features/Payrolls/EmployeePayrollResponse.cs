using System;

namespace PayrollProcessor.Functions.Features.Payrolls
{
    public class EmployeePayrollResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset CheckDate { get; set; }
        public string EmployeeDepartment { get; set; } = "";
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public string EmployeeStatus { get; set; } = "";
        public double GrossPayroll { get; set; }
        public string PayrollPeriod { get; set; } = "";
    }
}
