using Payroll.Processor.Data.Persistence.Features.Risks;
using System.Linq;

namespace Payroll.Processor.Data.Persistence.Context
{
    public interface IDbContext
    {
        IQueryable<RiskRecord> Risks { get; }
    }
}
