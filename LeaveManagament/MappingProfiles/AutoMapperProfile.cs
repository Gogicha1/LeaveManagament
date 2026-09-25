using AutoMapper;
using LeaveManagament.Data;
using LeaveManagament.Models.LeaveRequests;
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

        CreateMap<LeaveRequestCreateVM, LeaveRequest>();

        CreateMap<LeaveRequest, LeaveRequestReadOnlyVM>()
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name));

        CreateMap<LeaveRequest, LeaveRequestReviewVM>()
            .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
            .ForMember(dest => dest.EmployeeName,
                opt => opt.MapFrom(src => src.RequestingEmployee.FirstName + " " + src.RequestingEmployee.LastName))
            .ForMember(dest => dest.EmployeeEmail, opt => opt.MapFrom(src => src.RequestingEmployee.Email))
            .ForMember(dest => dest.ReviewerName,
                opt => opt.MapFrom(src => src.Reviewer == null
                    ? null
                    : src.Reviewer.FirstName + " " + src.Reviewer.LastName));
    }
}
