using AutoMapper;
using Payroll.Processor.Data.Domain.Features.Employees;

namespace Payroll.Processor.Data.Persistence.Features.Employees
{
    public class EmployeeRecordMappingProfile : Profile
    {
        public EmployeeRecordMappingProfile() =>
            CreateMap<EmployeeRecord, Employee>();
    }
}
