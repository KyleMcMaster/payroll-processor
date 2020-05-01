using AutoMapper;
using PayrollProcessor.Data.Domain.Features;

namespace PayrollProcessor.Data.Persistence.Features.Risks
{
    public class RiskRecordMappingProfile : Profile
    {
        public RiskRecordMappingProfile() =>
            CreateMap<RiskRecord, Risk>();
    }
}
