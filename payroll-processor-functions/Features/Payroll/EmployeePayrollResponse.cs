using System;

namespace PayrollProcessor.Functions.Features.Payroll
{
    public class EmployeePayrollResponse
    {
        public Guid Id { get; set; }
        public DateTimeOffset CheckDate { get; set; }
        public string EmployeeDepartment { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public double GrossPayroll { get; set; }
    }
}
