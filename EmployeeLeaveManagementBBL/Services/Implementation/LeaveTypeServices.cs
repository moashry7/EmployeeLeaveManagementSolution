using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveManagementBLL.Services.Implementation
{
    public class LeaveTypeServices : ILeaveTypeServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveTypeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(LeaveType leaveType, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveType>().Add(leaveType);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(LeaveType leaveType, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveType>().Update(leaveType);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(LeaveType leaveType, CancellationToken ct = default)
        {
            var hasLeaveRequests = await _unitOfWork.GetRepository<LeaveRequest>()
                .FindAsync(lr => lr.LeaveTypeId == leaveType.Id, ct);

            if (hasLeaveRequests.Any())
                throw new BusinessRuleException("Cannot delete a leave type that has existing leave requests.");

            _unitOfWork.GetRepository<LeaveType>().Delete(leaveType);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<LeaveType>> GetAllAsync(CancellationToken ct = default) =>
            await _unitOfWork.GetRepository<LeaveType>()
                .Query()
                .Include(lt => lt.LeaveRequests)
                .ToListAsync(ct);

        public async Task<LeaveType?> GetByIdAsync(int id, CancellationToken ct = default) =>
            await _unitOfWork.GetRepository<LeaveType>()
                .Query()
                .Include(lt => lt.LeaveRequests)
                .FirstOrDefaultAsync(lt => lt.Id == id, ct);
    }
}