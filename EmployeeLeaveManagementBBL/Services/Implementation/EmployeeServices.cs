using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementEntities.Enums;

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
            await ValidateEmailUniqueAsync(employee.Email, employee.Id, ct);
            ValidateJoinDate(employee.JoinDate);
            await ValidateDepartmentBudgetAsync(employee.DepartmentId, employee.Salary, employee.Id, ct);
            _unitOfWork.GetRepository<Employee>().Add(employee);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Employee employee, CancellationToken ct = default)
        {

            var hasPendingOrApproved = await _unitOfWork.GetRepository<LeaveRequest>()
                .FindAsync(lr => lr.EmployeeId == employee.Id &&
                                  (lr.Status == LeaveRequestStatus.Pending ||
                                   lr.Status == LeaveRequestStatus.Approved), ct);

            if (hasPendingOrApproved.Any())
                throw new BusinessRuleException("Cannot delete an employee who has pending or approved leave requests.");

            _unitOfWork.GetRepository<Employee>().Delete(employee);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public Task<IEnumerable<Employee>> GetAllAsync(CancellationToken ct = default) => _unitOfWork.GetRepository<Employee>().GetAllAsync(ct);



        public Task<Employee?> GetByIdAsync(int id, CancellationToken ct = default) => _unitOfWork.GetRepository<Employee>().GetByIdAsync(id, ct);

        public async Task UpdateAsync(Employee employee, CancellationToken ct = default)
        {
            await ValidateEmailUniqueAsync(employee.Email, employee.Id, ct);
            ValidateJoinDate(employee.JoinDate);
            await ValidateDepartmentBudgetAsync(employee.DepartmentId, employee.Salary, employee.Id, ct);

            _unitOfWork.GetRepository<Employee>().Update(employee);
            await _unitOfWork.SaveChangesAsync(ct);
        }



        #region Helper



        private async Task ValidateEmailUniqueAsync(string email, int currentEmployeeId, CancellationToken ct)
        {
            var duplicates = await _unitOfWork.GetRepository<Employee>()
                .FindAsync(e => e.Email == email && e.Id != currentEmployeeId, ct);

            if (duplicates.Any())
                throw new BusinessRuleException($"An employee with this email {email} already exists.");

        }

        private void ValidateJoinDate(DateTime joinDate)
        {
            if (joinDate.Date > DateTime.Now.Date)
                throw new BusinessRuleException("Join date cannot be in the future.");
        }

        private async Task ValidateDepartmentBudgetAsync(int departmentId, decimal newSalary, int currentEmployeeId, CancellationToken ct)
        {
            var department = await _unitOfWork.GetRepository<Department>().GetByIdAsync(departmentId, ct);
            if (department is null)
                throw new BusinessRuleException("Department not found.");

            var otherEmployeesSalarySum = _unitOfWork.GetRepository<Employee>()
                .Query()
                .Where(e => e.DepartmentId == departmentId && e.Id != currentEmployeeId)
                .Sum(e => (decimal?)e.Salary) ?? 0;

            var projectedTotal = otherEmployeesSalarySum + newSalary;

            if (projectedTotal > department.Budget)
                throw new BusinessRuleException(
                    $"Total salaries of the department ({projectedTotal:C}) will exceed the allocated budget ({department.Budget:C}).");
        }
    }

        #endregion
}
