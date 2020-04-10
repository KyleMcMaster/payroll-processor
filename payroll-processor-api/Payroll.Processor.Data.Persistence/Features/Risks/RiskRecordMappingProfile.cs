using AutoMapper;
using Payroll.Processor.Data.Domain.Features;

namespace Payroll.Processor.Data.Persistence.Features.Risks
{
    public class RiskRecordMappingProfile : Profile
    {
        public RiskRecordMappingProfile() =>
            CreateMap<RiskRecord, Risk>();
    }
}
