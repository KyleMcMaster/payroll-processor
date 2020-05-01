using PayrollProcessor.Data.Persistence.Features.Employees;
using PayrollProcessor.Data.Persistence.Features.Risks;
using System.Linq;

namespace PayrollProcessor.Data.Persistence.Context
{
    public class DbContext : IDbContext
    {
        public IQueryable<EmployeeRecord> Employees { get; }
        public IQueryable<RiskRecord> Risks { get; }

        public DbContext()
        {
            Employees = DataSeed.Employees();
            Risks = DataSeed.Risks();
        }
    }
}
