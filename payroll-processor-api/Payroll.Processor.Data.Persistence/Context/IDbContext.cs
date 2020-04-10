using Payroll.Processor.Data.Persistence.Features.Employees;
using Payroll.Processor.Data.Persistence.Features.Risks;
using System.Linq;

namespace Payroll.Processor.Data.Persistence.Context
{
    public interface IDbContext
    {
        IQueryable<EmployeeRecord> Employees { get; }
        IQueryable<RiskRecord> Risks { get; }
    }
}
