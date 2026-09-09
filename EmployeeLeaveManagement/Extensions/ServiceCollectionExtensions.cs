using EmployeeLeaveManagementWeb.MappingProfile;
using EmployeeLeaveManagementWeb.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EmployeeLeaveManagementWeb.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(EmployeeMappingProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(DepartmentMappingProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(LeaveTypeMappingProfile).Assembly);
            services.AddAutoMapper(cfg => { }, typeof(LeaveRequestMappingProfile).Assembly);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(options =>
               {
                   options.LoginPath = "/Account/Login";
                   options.AccessDeniedPath = "/Account/AccessDenied";
                   options.ExpireTimeSpan = TimeSpan.FromHours(8);
                   options.SlidingExpiration = true;
               });
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;



            return services;
        }
    }
}

