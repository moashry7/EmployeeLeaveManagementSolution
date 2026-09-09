using AutoMapper;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.LeaveRequestVM;

namespace EmployeeLeaveManagementWeb.MappingProfiles
{
    public class LeaveRequestMappingProfile : Profile
    {
        public LeaveRequestMappingProfile()
        {
            CreateMap<LeaveRequest, LeaveRequestListVM>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Employee.Department.Name))
                .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name));

            CreateMap<LeaveRequest, LeaveRequestDetailsVM>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.LeaveTypeName, opt => opt.MapFrom(src => src.LeaveType.Name))
                .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedBy != null ? src.ApprovedBy.FullName : null));

            CreateMap<LeaveRequestCreateVM, LeaveRequest>()
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveType, opt => opt.Ignore());

            CreateMap<LeaveRequest, LeaveRequestEditVM>()
                .ForMember(dest => dest.Employees, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveTypes, opt => opt.Ignore());

            CreateMap<LeaveRequestEditVM, LeaveRequest>()
                .ForMember(dest => dest.Employee, opt => opt.Ignore())
                .ForMember(dest => dest.LeaveType, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedById, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore());
        }
    }
}