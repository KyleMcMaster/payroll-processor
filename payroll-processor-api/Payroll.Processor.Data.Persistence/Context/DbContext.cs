using Payroll.Processor.Data.Persistence.Features;
using System.Linq;

namespace Payroll.Processor.Data.Persistence.Context
{
    public class DbContext : IDbContext
    {
        public IQueryable<RiskRecord> Risks { get; }

        public DbContext()
        {
            Risks = ContextDataSeed.SeedRisks().AsQueryable();
        }
    }
}
