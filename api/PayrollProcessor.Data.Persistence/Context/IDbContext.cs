using PayrollProcessor.Data.Persistence.Features.Employees;
using PayrollProcessor.Data.Persistence.Features.Risks;
using System.Linq;

namespace PayrollProcessor.Data.Persistence.Context
{
    public interface IDbContext
    {
        IQueryable<EmployeeRecord> Employees { get; }
        IQueryable<RiskRecord> Risks { get; }
    }
}
