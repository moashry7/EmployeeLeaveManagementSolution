using System.Security.Claims;
using AutoMapper;
using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementEntities.Enums;
using EmployeeLeaveManagementWeb.ViewModels.LeaveRequestVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeLeaveManagementWeb.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestServices _leaveRequestServices;
        private readonly IEmployeeServices _employeeServices;
        private readonly ILeaveTypeServices _leaveTypeServices;
        private readonly IMapper _mapper;

        public LeaveRequestController(
            ILeaveRequestServices leaveRequestServices,
            IEmployeeServices employeeServices,
            ILeaveTypeServices leaveTypeServices,
            IMapper mapper)
        {
            _leaveRequestServices = leaveRequestServices;
            _employeeServices = employeeServices;
            _leaveTypeServices = leaveTypeServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var currentId = GetCurrentEmployeeId();
            var role = GetCurrentRole();

            var allRequests = await _leaveRequestServices.GetAllAsync(ct);

            IEnumerable<LeaveRequest> visibleRequests = role switch
            {
                EmployeeRole.Admin => allRequests,

                EmployeeRole.Manager => await FilterByManagerDepartmentAsync(allRequests, currentId, ct),

                _ => allRequests.Where(r => r.EmployeeId == currentId)
            };

            var vm = _mapper.Map<IEnumerable<LeaveRequestListVM>>(visibleRequests)
                .OrderByDescending(r => r.StartDate);

            return View(vm);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var request = await _leaveRequestServices.GetByIdAsync(id, ct);
            if (request == null) return NotFound();

            var vm = _mapper.Map<LeaveRequestDetailsVM>(request);
            return View(vm);
        }

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new LeaveRequestCreateVM();
            await PopulateDropdownsAsync(vm, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequestCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(vm, ct);
                return View(vm);
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(vm);

            try
            {
                await _leaveRequestServices.AddAsync(leaveRequest, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDropdownsAsync(vm, ct);
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var request = await _leaveRequestServices.GetByIdAsync(id, ct);
            if (request == null) return NotFound();

            var vm = _mapper.Map<LeaveRequestEditVM>(request);
            await PopulateDropdownsAsync(vm, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveRequestEditVM vm, CancellationToken ct)
        {
            if (id != vm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(vm, ct);
                return View(vm);
            }

            var leaveRequest = _mapper.Map<LeaveRequest>(vm);

            try
            {
                await _leaveRequestServices.UpdateAsync(leaveRequest, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateDropdownsAsync(vm, ct);
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var request = await _leaveRequestServices.GetByIdAsync(id, ct);
            if (request == null) return NotFound();

            var vm = _mapper.Map<LeaveRequestDetailsVM>(request);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var request = await _leaveRequestServices.GetByIdAsync(id, ct);
            if (request == null) return NotFound();

            try
            {
                await _leaveRequestServices.DeleteAsync(request, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id, CancellationToken ct)
        {
            try
            {
                var approverId = GetCurrentEmployeeId();
                await _leaveRequestServices.ApproveAsync(id, approverId, ct);
            }
            catch (BusinessRuleException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id, CancellationToken ct)
        {
            try
            {
                var approverId = GetCurrentEmployeeId();
                await _leaveRequestServices.RejectAsync(id, approverId, ct);
            }
            catch (BusinessRuleException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        #region Helpers

        private int GetCurrentEmployeeId()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim!);
        }

        private EmployeeRole GetCurrentRole()
        {
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.Parse<EmployeeRole>(roleClaim!);
        }

        private async Task<IEnumerable<LeaveRequest>> FilterByManagerDepartmentAsync(
            IEnumerable<LeaveRequest> allRequests, int managerId, CancellationToken ct)
        {
            var manager = await _employeeServices.GetByIdAsync(managerId, ct);
            if (manager == null) return Enumerable.Empty<LeaveRequest>();

            return allRequests.Where(r => r.Employee.DepartmentId == manager.DepartmentId);
        }

        private async Task PopulateDropdownsAsync(LeaveRequestCreateVM vm, CancellationToken ct)
        {
            var currentId = GetCurrentEmployeeId();
            var role = GetCurrentRole();

            var employees = await GetVisibleEmployeesAsync(role, currentId, ct);
            var leaveTypes = await _leaveTypeServices.GetAllAsync(ct);

            vm.Employees = employees.Select(e => new SelectListItem(e.FullName, e.Id.ToString())).ToList();
            vm.LeaveTypes = leaveTypes.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();

            // A plain Employee can only submit a request for themselves — lock the value.
            if (role == EmployeeRole.Employee)
                vm.EmployeeId = currentId;
        }

        private async Task PopulateDropdownsAsync(LeaveRequestEditVM vm, CancellationToken ct)
        {
            var currentId = GetCurrentEmployeeId();
            var role = GetCurrentRole();

            var employees = await GetVisibleEmployeesAsync(role, currentId, ct);
            var leaveTypes = await _leaveTypeServices.GetAllAsync(ct);

            vm.Employees = employees.Select(e => new SelectListItem(e.FullName, e.Id.ToString())).ToList();
            vm.LeaveTypes = leaveTypes.Select(t => new SelectListItem(t.Name, t.Id.ToString())).ToList();
        }

        private async Task<IEnumerable<Employee>> GetVisibleEmployeesAsync(EmployeeRole role, int currentId, CancellationToken ct)
        {
            switch (role)
            {
                case EmployeeRole.Admin:
                    return await _employeeServices.GetAllAsync(ct);

                case EmployeeRole.Manager:
                    var manager = await _employeeServices.GetByIdAsync(currentId, ct);
                    if (manager == null) return Enumerable.Empty<Employee>();
                    return await _employeeServices.GetByDepartmentAsync(manager.DepartmentId, ct);

                default: // Employee
                    var self = await _employeeServices.GetByIdAsync(currentId, ct);
                    return self == null ? Enumerable.Empty<Employee>() : new List<Employee> { self };
            }
        }

        #endregion
    }
}