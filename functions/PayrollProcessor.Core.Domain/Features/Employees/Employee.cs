using System;
using System.Collections.Generic;
using System.Linq;
using PayrollProcessor.Core.Domain.Features.Payrolls;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class Employee
    {
        public Guid Id { get; set; }
        public string Department { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTimeOffset EmploymentStartedOn { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Status { get; set; } = "";
        public string Title { get; set; } = "";
        public IEnumerable<EmployeePayroll> Payrolls { get; set; } = new EmployeePayroll[] { };
        public string Version { get; set; } = "";

        public Employee(Guid id) => Id = id;

        public void UpdatePayrolls(Payroll payroll)
        {
            var employeePayroll = new EmployeePayroll
            {
                Id = payroll.Id,
                CheckDate = payroll.CheckDate,
                GrossPayroll = payroll.GrossPayroll,
                PayrollPeriod = payroll.PayrollPeriod
            };

            if (Payrolls.Count() < 30)
            {
                Payrolls = Payrolls.Prepend(employeePayroll);

                return;
            }

            /*
             * Find and (if found) replace the least older of all payrolls
             * older than the one provided
             * This keeps the set attached to the Employee as the latest 30 by CheckDate
             */
            var leastOlderPayroll = Payrolls
                .Select(p => new { payroll = p, diff = (p.CheckDate - payroll.CheckDate).Ticks })
                .Where(obj => obj.diff > 0)
                .OrderBy(obj => obj.diff)
                .Select(obj => obj.payroll)
                .FirstOrDefault();

            if (leastOlderPayroll is object)
            {
                Payrolls = Payrolls.Where(p => p.Id != leastOlderPayroll.Id).Prepend(employeePayroll);
            }
        }
    }
}
