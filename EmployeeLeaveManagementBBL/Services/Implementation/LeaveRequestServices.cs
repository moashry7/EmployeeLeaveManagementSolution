using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementEntities.Enums;
using Microsoft.EntityFrameworkCore;

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
            ValidateDateRange(leaveRequest.StartDate, leaveRequest.EndDate);
            ValidateStartDateNotTooOld(leaveRequest.StartDate);
            await ValidateNoOverlapAsync(leaveRequest, ct);

            leaveRequest.Status = LeaveRequestStatus.Pending;
            leaveRequest.ApprovedById = null;

            _unitOfWork.GetRepository<LeaveRequest>().Add(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(LeaveRequest leaveRequest, CancellationToken ct = default)
        {
            var existing = await _unitOfWork.GetRepository<LeaveRequest>().GetByIdAsync(leaveRequest.Id, ct);
            if (existing is null)
                throw new BusinessRuleException("The leave request does not exist.");

            if (existing.Status != LeaveRequestStatus.Pending)
                throw new BusinessRuleException("A request that has already been approved or rejected cannot be modified.");

            ValidateDateRange(leaveRequest.StartDate, leaveRequest.EndDate);
            ValidateStartDateNotTooOld(leaveRequest.StartDate);
            await ValidateNoOverlapAsync(leaveRequest, ct);

            _unitOfWork.GetRepository<LeaveRequest>().Update(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(LeaveRequest leaveRequest, CancellationToken ct = default)
        {
            _unitOfWork.GetRepository<LeaveRequest>().Delete(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public Task<IEnumerable<LeaveRequest>> GetAllAsync(CancellationToken ct = default)
            => _unitOfWork.GetRepository<LeaveRequest>().GetAllAsync(ct);

        public Task<LeaveRequest?> GetByIdAsync(int id, CancellationToken ct = default)
            => _unitOfWork.GetRepository<LeaveRequest>().GetByIdAsync(id, ct);

        public async Task ApproveAsync(int leaveRequestId, int approverId, CancellationToken ct = default)
        {
            var leaveRequest = await GetPendingRequestOrThrowAsync(leaveRequestId, ct);
            var approver = await ValidateApproverOwnsDepartmentAsync(leaveRequest, approverId, ct);

            await ValidateYearlyDaysAllowedAsync(leaveRequest, ct);

            leaveRequest.Status = LeaveRequestStatus.Approved;
            leaveRequest.ApprovedById = approver.Id;

            _unitOfWork.GetRepository<LeaveRequest>().Update(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task RejectAsync(int leaveRequestId, int approverId, CancellationToken ct = default)
        {
            var leaveRequest = await GetPendingRequestOrThrowAsync(leaveRequestId, ct);
            var approver = await ValidateApproverOwnsDepartmentAsync(leaveRequest, approverId, ct);

            leaveRequest.Status = LeaveRequestStatus.Rejected;
            leaveRequest.ApprovedById = approver.Id;

            _unitOfWork.GetRepository<LeaveRequest>().Update(leaveRequest);
            await _unitOfWork.SaveChangesAsync(ct);
        }


        #region Helper


        private void ValidateDateRange(DateTime start, DateTime end)
        {
            if (end.Date < start.Date)
                throw new BusinessRuleException("The end date cannot be before the start date.");
        }

        private void ValidateStartDateNotTooOld(DateTime start)
        {
            if (start.Date < DateTime.Now.Date.AddYears(-1))
                throw new BusinessRuleException("A leave request cannot be submitted if it begins more than a year from today..");
        }

        private async Task ValidateNoOverlapAsync(LeaveRequest leaveRequest, CancellationToken ct)
        {
            var overlapping = _unitOfWork.GetRepository<LeaveRequest>()
                .Query()
                .Where(lr => lr.EmployeeId == leaveRequest.EmployeeId
                          && lr.Id != leaveRequest.Id
                          && lr.Status != LeaveRequestStatus.Rejected
                          && lr.StartDate <= leaveRequest.EndDate
                          && lr.EndDate >= leaveRequest.StartDate);

            if (await overlapping.AnyAsync(ct))
                throw new BusinessRuleException("There is already a leave request overlapping with these dates for the same employee.");
        }

        private async Task<LeaveRequest> GetPendingRequestOrThrowAsync(int leaveRequestId, CancellationToken ct)
        {
            var leaveRequest = await _unitOfWork.GetRepository<LeaveRequest>().GetByIdAsync(leaveRequestId, ct);
            if (leaveRequest is null)
                throw new BusinessRuleException("The leave request does not exist.");

            if (leaveRequest.Status != LeaveRequestStatus.Pending)
                throw new BusinessRuleException("A decision has already been made regarding this request.");

            return leaveRequest;
        }

        private async Task<Employee> ValidateApproverOwnsDepartmentAsync(LeaveRequest leaveRequest, int approverId, CancellationToken ct)
        {
            var approver = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(approverId, ct);
            if (approver is null)
                throw new BusinessRuleException("The employee responsible for approval does not exist.");

            var requestingEmployee = await _unitOfWork.GetRepository<Employee>().GetByIdAsync(leaveRequest.EmployeeId, ct);
            if (requestingEmployee is null)
                throw new BusinessRuleException("The employee who submitted the request does not exist.");

            var department = await _unitOfWork.GetRepository<Department>().GetByIdAsync(requestingEmployee.DepartmentId, ct);
            if (department is null || department.ManagerId != approver.Id)
                throw new BusinessRuleException("You can only approve requests from employees in your department.");

            return approver;
        }

        private async Task ValidateYearlyDaysAllowedAsync(LeaveRequest leaveRequest, CancellationToken ct)
        {
            var leaveType = await _unitOfWork.GetRepository<LeaveType>().GetByIdAsync(leaveRequest.LeaveTypeId, ct);
            if (leaveType is null)
                throw new BusinessRuleException("The leave type does not exist.");

            var year = leaveRequest.StartDate.Year;

            var approvedDaysThisYear = await _unitOfWork.GetRepository<LeaveRequest>()
                .Query()
                .Where(lr => lr.EmployeeId == leaveRequest.EmployeeId
                          && lr.LeaveTypeId == leaveRequest.LeaveTypeId
                          && lr.Status == LeaveRequestStatus.Approved
                          && lr.StartDate.Year == year)
                .ToListAsync(ct);

            var usedDays = approvedDaysThisYear.Sum(lr => (lr.EndDate - lr.StartDate).Days + 1);
            var newRequestDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;

            if (usedDays + newRequestDays > leaveType.DaysAllowedPerYear)
                throw new BusinessRuleException(
                    $"This request will exceed the annual allowance ({leaveType.DaysAllowedPerYear} days) for this leave type.");
        }
    }


        #endregion

}


