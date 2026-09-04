using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementBLL.Services.Implementation
{
    public class LeaveRequestServices : ILeaveRequestServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeaveRequestServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(LeaveRequest leaveRequest, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveRequest>().Add(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(LeaveRequest leaveRequest, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveRequest>().Delete(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllAsync(CancellationToken ct = default) => await _unitOfWork.GetRepository<LeaveRequest>().GetAllAsync(ct);


        public async Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken ct = default) => await _unitOfWork.GetRepository<LeaveRequest>().GetByIdAsync(id, ct);


        public async Task UpdateAsync(LeaveRequest leaveRequest, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveRequest>().Update(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}

