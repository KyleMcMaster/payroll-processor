using Payroll.Processor.Data.Persistence.Features.Employees;
using Payroll.Processor.Data.Persistence.Features.Risks;
using System.Linq;

namespace Payroll.Processor.Data.Persistence.Context
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
