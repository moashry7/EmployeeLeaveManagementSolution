using AutoMapper;
using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.DepartmentVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeLeaveManagementWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentServices _departmentServices;
        private readonly IEmployeeServices _employeeServices;
        private readonly IMapper _mapper;

        public DepartmentController(
            IDepartmentServices departmentServices,
            IEmployeeServices employeeServices,
            IMapper mapper)
        {
            _departmentServices = departmentServices;
            _employeeServices = employeeServices;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var departments = await _departmentServices.GetAllAsync(ct);
            var vm = _mapper.Map<IEnumerable<DepartmentListVM>>(departments);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var department = await _departmentServices.GetByIdAsync(id, ct);
            if (department is null)
                return NotFound();

            var vm = _mapper.Map<DepartmentDetailsVM>(department);
            return View(vm);
        }

        public IActionResult Create()
        {
            return View(new DepartmentCreateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var department = _mapper.Map<Department>(vm);

            try
            {
                await _departmentServices.AddAsync(department, ct);
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
            var department = await _departmentServices.GetByIdAsync(id, ct);
            if (department is null)
                return NotFound();

            var vm = _mapper.Map<DepartmentEditVM>(department);
            vm.Employees = await GetDepartmentEmployeesSelectListAsync(id, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DepartmentEditVM vm, CancellationToken ct)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.Employees = await GetDepartmentEmployeesSelectListAsync(id, ct);
                return View(vm);
            }

            var department = _mapper.Map<Department>(vm);

            try
            {
                await _departmentServices.UpdateAsync(department, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                vm.Employees = await GetDepartmentEmployeesSelectListAsync(id, ct);
                return View(vm);
            }
        }

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var department = await _departmentServices.GetByIdAsync(id, ct);
            if (department is null)
                return NotFound();

            var vm = _mapper.Map<DepartmentDetailsVM>(department);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var department = await _departmentServices.GetByIdAsync(id, ct);
            if (department is null)
                return NotFound();

            try
            {
                await _departmentServices.DeleteAsync(department, ct);
            }
            catch (BusinessRuleException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<IEnumerable<SelectListItem>> GetDepartmentEmployeesSelectListAsync(int departmentId, CancellationToken ct)
        {
            var employees = await _employeeServices.GetByDepartmentAsync(departmentId, ct);
            return employees.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.FullName
            });
        }
    }
}