using EmployeeLeaveManagementEntities.Entities;

namespace EmployeeLeaveManagementBLL.Services.Interfaces
{
    public interface IReportServices
    {
        Task<IEnumerable<Department>> GetDepartmentReportDataAsync(CancellationToken ct = default);
        Task<IEnumerable<Employee>> GetLeaveSummaryDataAsync(CancellationToken ct = default);
    }
}