using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;

namespace EmployeeLeaveManagementBLL.Services.Implementation
{
    public class EmployeeServices : IEmployeeServices

    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Employee employee, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<Employee>().Add(employee);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Employee employee, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<Employee>().Delete(employee);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public Task<IEnumerable<Employee>> GetAllAsync(CancellationToken ct = default) => _unitOfWork.GetRepository<Employee>().GetAllAsync(ct);



        public Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default) => _unitOfWork.GetRepository<Employee>().GetByIdAsync(id, ct);

        public async Task UpdateAsync(Employee employee, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<Employee>().Update(employee);
            await _unitOfWork.SaveChangesAsync(ct);


        }

    }
}
