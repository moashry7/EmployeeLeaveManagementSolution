using EmployeeLeaveManagementEntities.Entities;

namespace EmployeeLeaveManagementBLL.Services.Interfaces
{
    public interface IDepartmentServices
    {
        Task<IEnumerable<Department>> GetAllAsync(CancellationToken ct = default);
        Task<Department?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(Department department, CancellationToken ct = default);
        Task UpdateAsync(Department department, CancellationToken ct = default);
        Task DeleteAsync(Department department, CancellationToken ct = default);
    }
}
