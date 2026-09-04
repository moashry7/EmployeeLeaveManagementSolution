using EmployeeLeaveManagementEntities.Entities;

namespace EmployeeLeaveManagementBLL.Services.Interfaces
{
    public interface ILeaveTypeServices
    {
        Task<IEnumerable<LeaveType>> GetAllAsync(CancellationToken ct = default);
        Task<LeaveType?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(LeaveType leaveType, CancellationToken ct = default);
        Task UpdateAsync(LeaveType leaveType, CancellationToken ct = default);
        Task DeleteAsync(LeaveType leaveType, CancellationToken ct = default);    
    }
}
