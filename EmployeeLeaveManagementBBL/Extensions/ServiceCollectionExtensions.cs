using EmployeeLeaveManagementBLL.Services.Implementation;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeLeaveManagementBLL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(
            this IServiceCollection services)
        {
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<ILeaveTypeServices, LeaveTypeServices>();
            services.AddScoped<ILeaveRequestServices, LeaveRequestServices>();

            return services;
        }
    }
}