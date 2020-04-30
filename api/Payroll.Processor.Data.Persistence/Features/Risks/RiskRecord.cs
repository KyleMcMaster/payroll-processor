using System;

namespace Payroll.Processor.Data.Persistence.Features.Risks
{
    public class RiskRecord
    {
        public Guid Id { get; set; }
        public string CodeName { get; set; }
        public string DisplayName { get; set; }
    }
}
