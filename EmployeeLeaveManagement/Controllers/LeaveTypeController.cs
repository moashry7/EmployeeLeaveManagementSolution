using AutoMapper;
using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.LeaveTypeVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagementWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveTypeServices _leaveTypeServices;
        private readonly IMapper _mapper;

        public LeaveTypeController(ILeaveTypeServices leaveTypeServices, IMapper mapper)
        {
            _leaveTypeServices = leaveTypeServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var leaveTypes = await _leaveTypeServices.GetAllAsync(ct);
            var vm = _mapper.Map<IEnumerable<LeaveTypeListVM>>(leaveTypes);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id, ct);
            if (leaveType == null) return NotFound();

            var vm = _mapper.Map<LeaveTypeDetailsVM>(leaveType);
            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new LeaveTypeCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveTypeCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var leaveType = _mapper.Map<LeaveType>(vm);

            try
            {
                await _leaveTypeServices.AddAsync(leaveType, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id, ct);
            if (leaveType == null) return NotFound();

            var vm = _mapper.Map<LeaveTypeEditVM>(leaveType);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LeaveTypeEditVM vm, CancellationToken ct)
        {
            if (id != vm.Id) return NotFound();
            if (!ModelState.IsValid) return View(vm);

            var leaveType = _mapper.Map<LeaveType>(vm);

            try
            {
                await _leaveTypeServices.UpdateAsync(leaveType, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id, ct);
            if (leaveType == null) return NotFound();

            var vm = _mapper.Map<LeaveTypeDetailsVM>(leaveType);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var leaveType = await _leaveTypeServices.GetByIdAsync(id, ct);
            if (leaveType == null) return NotFound();

            try
            {
                await _leaveTypeServices.DeleteAsync(leaveType, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }
    }
}