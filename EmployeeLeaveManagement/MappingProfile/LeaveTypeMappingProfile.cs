using AutoMapper;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.LeaveTypeVM;

namespace EmployeeLeaveManagementWeb.MappingProfiles
{
    public class LeaveTypeMappingProfile : Profile
    {
        public LeaveTypeMappingProfile()
        {
            CreateMap<LeaveType, LeaveTypeListVM>()
                .ForMember(dest => dest.LeaveRequestCount,
                    opt => opt.MapFrom(src => src.LeaveRequests.Count));

            CreateMap<LeaveType, LeaveTypeDetailsVM>()
                .ForMember(dest => dest.LeaveRequestCount,
                    opt => opt.MapFrom(src => src.LeaveRequests.Count));

            CreateMap<LeaveTypeCreateVM, LeaveType>();

            CreateMap<LeaveType, LeaveTypeEditVM>();
            CreateMap<LeaveTypeEditVM, LeaveType>()
                .ForMember(dest => dest.LeaveRequests, opt => opt.Ignore());
        }
    }
}