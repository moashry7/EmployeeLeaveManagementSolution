using AutoMapper;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.EmployeeVM;

namespace EmployeeLeaveManagementWeb.MappingProfile
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<Employee, EmployeeListVM>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Employee, EmployeeDetailsVM>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Employee, EmployeeEditVM>();

            CreateMap<EmployeeCreateVM, Employee>();
            
            CreateMap<EmployeeEditVM, Employee>();

        }
    }
}
