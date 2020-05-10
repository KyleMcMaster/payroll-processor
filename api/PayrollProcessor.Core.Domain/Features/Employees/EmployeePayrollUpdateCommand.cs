using System;
using Ardalis.GuardClauses;
using PayrollProcessor.Core.Domain.Intrastructure.Operations.Commands;

namespace PayrollProcessor.Core.Domain.Features.Employees
{
    public class EmployeePayrollUpdateCommand : ICommand<EmployeePayroll>
    {
        public Guid EmployeeId { get; }
        public Guid EmployeePayrollId { get; }
        public DateTimeOffset CheckDate { get; }
        public decimal GrossPayroll { get; }
        public string PayrollPeriod { get; }
        public EmployeePayroll EntityToUpdate { get; }

        public EmployeePayrollUpdateCommand(DateTimeOffset checkDate, decimal grossPayroll, string payrollPeriod, EmployeePayroll entityToUpdate)
        {
            Guard.Against.Null(entityToUpdate, nameof(entityToUpdate));

            EntityToUpdate = entityToUpdate;
            PayrollPeriod = payrollPeriod;
            GrossPayroll = grossPayroll;
            CheckDate = checkDate;
        }
    }
}
