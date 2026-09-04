using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementBLL.Services.Implementation
{
    public class DepartmentServices : IDepartmentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddAsync(Department department, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<Department>().Add(department);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Department department, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<Department>().Delete(department);
            await _unitOfWork.SaveChangesAsync(ct);
        }


        public Task<IEnumerable<Department>> GetAllAsync(CancellationToken ct = default) => _unitOfWork.GetRepository<Department>().GetAllAsync(ct);


        public Task<Department?> GetByIdAsync(int id, CancellationToken ct = default) => _unitOfWork.GetRepository<Department>().GetByIdAsync(id, ct);

        public async Task UpdateAsync(Department department, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<Department>().Update(department);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
