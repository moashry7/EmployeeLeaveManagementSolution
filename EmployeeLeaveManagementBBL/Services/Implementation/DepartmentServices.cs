using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;

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
            await ValidateBudgetCoversSalariesAsync(department, ct);

            _unitOfWork.GetRepository<Department>().Add(department);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Department department, CancellationToken ct = default)
        {
            await ValidateBudgetCoversSalariesAsync(department, ct);

            _unitOfWork.GetRepository<Department>().Update(department);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Department department, CancellationToken ct = default)
        {
            var hasEmployees = await _unitOfWork.GetRepository<Employee>()
                .FindAsync(e => e.DepartmentId == department.Id, ct);

            if (hasEmployees.Any())
                throw new BusinessRuleException("Cannot delete a department that has employees. Reassign them first.");

            _unitOfWork.GetRepository<Department>().Delete(department);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public Task<IEnumerable<Department>> GetAllAsync(CancellationToken ct = default)
            => _unitOfWork.GetRepository<Department>().GetAllAsync(ct);

        public Task<Department?> GetByIdAsync(int id, CancellationToken ct = default)
            => _unitOfWork.GetRepository<Department>().GetByIdAsync(id, ct);

        #region Helper
        private async Task ValidateBudgetCoversSalariesAsync(Department department, CancellationToken ct)
        {
            var currentTotalSalaries = await _unitOfWork.GetRepository<Employee>()
                .Query()
                .Where(e => e.DepartmentId == department.Id)
                .SumAsync(e => (decimal?)e.Salary, ct) ?? 0;

            if (department.Budget < currentTotalSalaries)
                throw new BusinessRuleException(
                    $"Budget ({department.Budget:C}) cannot be lower than the current total salaries ({currentTotalSalaries:C}).");

        }
    }
}

        #endregion