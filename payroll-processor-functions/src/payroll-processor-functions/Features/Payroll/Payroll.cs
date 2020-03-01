using System;

namespace PayrollProcessor.Functions.Features.Payroll
{
    public class Payroll
    {
        public Guid Id { get; set; }
        public DateTimeOffset CheckDate { get; set; }
        public Guid EmployeeId { get; set; }
        public double GrossPayroll { get; set; }
    }
}
