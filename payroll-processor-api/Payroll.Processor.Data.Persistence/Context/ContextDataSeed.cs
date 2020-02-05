using Payroll.Processor.Data.Persistence.Features;
using System;

namespace Payroll.Processor.Data.Persistence.Context
{
    public static class ContextDataSeed
    {
        public static RiskRecord[] SeedRisks()
        {
            return new RiskRecord[]
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
                },
            };
        }
    }
}
