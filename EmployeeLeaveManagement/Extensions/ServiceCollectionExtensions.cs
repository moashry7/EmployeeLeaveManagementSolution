using EmployeeLeaveManagementWeb.MappingProfile;

namespace EmployeeLeaveManagementWeb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(EmployeeMappingProfile).Assembly);

            return services;
        }
    }
}

