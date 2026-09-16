using AutoMapper;
using LeaveManagament.Data;
using LeaveManagament.Models.LeaveTypes;

namespace LeaveManagament.MappingProfiles;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<LeaveType, LeaveTypeReadOnlyVM>();
        CreateMap<LeaveTypeCreateVM, LeaveType>();
        CreateMap<LeaveTypeEditVM, LeaveType>().ReverseMap(); 
            // .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            // .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            // .ForMember(dest => dest.NumberOfDays, opt => opt.MapFrom(src => src.NumberOfDays));
    }
}