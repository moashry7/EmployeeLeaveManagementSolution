using EmployeeLeaveManagementEntities.Entities;

namespace EmployeeLeaveManagementBLL.Services.Interfaces
{
    public interface ILeaveRequestServices
    {
        Task<IEnumerable<LeaveRequest>> GetAllAsync(CancellationToken ct = default);
        Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
        Task UpdateAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
        Task DeleteAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
        Task ApproveAsync(int leaveRequestId, int approverId, CancellationToken ct = default);
        Task RejectAsync(int leaveRequestId, int approverId, CancellationToken ct = default);

    }
}
