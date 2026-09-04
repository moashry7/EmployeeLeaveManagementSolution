using EmployeeLeaveManagementEntities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementBLL.Services.Interfaces
{
    public interface IEmployeeServices
    {
        Task<IEnumerable<Employee>> GetAllAsync(CancellationToken ct = default);
        Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddAsync(Employee employee, CancellationToken ct = default);
        Task UpdateAsync(Employee employee, CancellationToken ct = default);
        Task DeleteAsync(Employee employee, CancellationToken ct = default);
    }
}
