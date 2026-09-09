using AutoMapper;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.DepartmentVM;

public class DepartmentMappingProfile : Profile
{
    public DepartmentMappingProfile()
    {
        CreateMap<Department, DepartmentListVM>()
            .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count))
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null));

        CreateMap<Department, DepartmentDetailsVM>()
            .ForMember(dest => dest.ManagerName, opt => opt.MapFrom(src => src.Manager != null ? src.Manager.FullName : null))
            .ForMember(dest => dest.EmployeeNames, opt => opt.MapFrom(src => src.Employees.Select(e => e.FullName).ToList()));

        CreateMap<Department, DepartmentEditVM>()
            .ForMember(dest => dest.Employees, opt => opt.Ignore());   

        CreateMap<DepartmentCreateVM, Department>();
        CreateMap<DepartmentEditVM, Department>();
    }
}