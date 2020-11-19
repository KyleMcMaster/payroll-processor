using System;
using System.Globalization;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class EmployeePayrollNew
    {
        public DateTimeOffset CheckDate { get; set; }
        public decimal GrossPayroll { get; set; }
        public string PayrollPeriod => (ISOWeek.GetWeekOfYear(CheckDate.DateTime) / 2).ToString().PadLeft(2, '0');
    }
}
