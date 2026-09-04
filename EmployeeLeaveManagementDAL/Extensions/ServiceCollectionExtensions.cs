using EmployeeLeaveManagementDAL.Data.Dbcontext;
using EmployeeLeaveManagementDAL.Data.Repositories.Implementation;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace EmployeeLeaveManagementDAL.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccess(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}

