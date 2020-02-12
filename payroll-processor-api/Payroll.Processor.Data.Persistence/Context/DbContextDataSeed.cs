using Payroll.Processor.Data.Persistence.Features;
using Payroll.Processor.Data.Persistence.Features.Risks;
using System;
using System.Linq;

namespace Payroll.Processor.Data.Persistence.Context
{
    public class DbContextDataSeed
    {
        public IQueryable<RiskRecord> Risks { get; }

        public DbContextDataSeed()
        {
            Risks = GenerateRisks();
        }

        private IQueryable<RiskRecord> GenerateRisks()
        {
            var risks = new RiskRecord[]
            {
                new RiskRecord
                {
                    Id = Guid.NewGuid(),
                    CodeName = "LOW",
                    DisplayName = "Low"
                },
                new RiskRecord
                {
                    Id = Guid.NewGuid(),
                    CodeName = "MEDIUM",
                    DisplayName = "Medium"
                },
                new RiskRecord
                {
                    Id = Guid.NewGuid(),
                    CodeName = "HIGH",
                    DisplayName = "High"
                }
            };

            return risks.AsQueryable();
        }
    }
}
