using AutoMapper;
using PayrollProcessor.Data.Domain.Features.Employees;

namespace PayrollProcessor.Data.Persistence.Features.Employees
{
    public class EmployeeRecordMappingProfile : Profile
    {
        public EmployeeRecordMappingProfile() =>
            CreateMap<EmployeeRecord, Employee>();
    }
}
