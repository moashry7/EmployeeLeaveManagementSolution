using EmployeeLeaveManagementEntities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementBLL.Services.Interfaces
{
    public interface ILeaveRequestServices
    {
        Task<IEnumerable<LeaveRequest>> GetAllAsync(CancellationToken ct = default);
        Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
        Task UpdateAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
        Task DeleteAsync(LeaveRequest leaveRequest, CancellationToken ct = default);
    }
}
