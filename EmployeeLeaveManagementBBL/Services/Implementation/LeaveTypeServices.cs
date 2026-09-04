using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

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
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(LeaveType leaveType, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveType>().Delete(leaveType);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<LeaveType>> GetAllAsync(CancellationToken ct = default) => await _unitOfWork.GetRepository<LeaveType>().GetAllAsync(ct);


        public async Task<LeaveType?> GetByIdAsync(int id, CancellationToken ct = default) => await _unitOfWork.GetRepository<LeaveType>().GetByIdAsync(id, ct);

        public async Task UpdateAsync(LeaveType leaveType, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveType>().Update(leaveType);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
