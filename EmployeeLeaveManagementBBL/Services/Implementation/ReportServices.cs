using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementBLL.Services.Implementation
{
    public class ReportServices : IReportServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Department>> GetDepartmentReportDataAsync(CancellationToken ct = default) =>
            await _unitOfWork.GetRepository<Department>()
                .Query()
                .Include(d => d.Employees)
                .ToListAsync(ct);
        public async Task<IEnumerable<Employee>> GetLeaveSummaryDataAsync(CancellationToken ct = default) =>
            await _unitOfWork.GetRepository<Employee>()
                .Query()
                .Include(e => e.Department)
                .Include(e => e.LeaveRequests)
                .ToListAsync(ct);
    }
}