using Payroll.Processor.Data.Persistence.Features.Risks;
using System.Linq;

namespace Payroll.Processor.Data.Persistence.Context
{
    public class DbContext : IDbContext
    {
        public IQueryable<RiskRecord> Risks { get; }

        public DbContext()
        {
            var seedData = new DbContextDataSeed();

            Risks = seedData.Risks;
        }
    }
}
