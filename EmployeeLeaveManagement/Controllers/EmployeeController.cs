using AutoMapper;
using EmployeeLeaveManagementBLL.Exceptions;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementWeb.ViewModels.EmployeeVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeLeaveManagementWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly IMapper _mapper;

        public EmployeeController(
            IEmployeeServices employeeServices,
            IDepartmentServices departmentServices,
            IMapper mapper)
        {
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
            _mapper = mapper;
        }

        #region Home paga

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var employees = await _employeeServices.GetAllAsync(ct);
            var vm = _mapper.Map<IEnumerable<EmployeeListVM>>(employees);
            return View(vm);
        }
        #endregion

        #region Employee Details

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var employee = await _employeeServices.GetByIdAsync(id, ct);
            if (employee is null)
                return NotFound();

            var vm = _mapper.Map<EmployeeDetailsVM>(employee);
            return View(vm);
        }

        #endregion

        #region Employee Create

        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new EmployeeCreateVM
            {
                Departments = await GetDepartmentsSelectListAsync(ct)
            };
            return View(vm);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = await GetDepartmentsSelectListAsync(ct);
                return View(vm);
            }

            var employee = _mapper.Map<Employee>(vm);

            try
            {
                await _employeeServices.AddAsync(employee, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                vm.Departments = await GetDepartmentsSelectListAsync(ct);
                return View(vm);
            }
        }


        #endregion        // GET: /Employee/Edit/5

        #region Employee Edit

        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var employee = await _employeeServices.GetByIdAsync(id, ct);
            if (employee is null)
                return NotFound();

            var vm = _mapper.Map<EmployeeEditVM>(employee);
            vm.Departments = await GetDepartmentsSelectListAsync(ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeEditVM vm, CancellationToken ct)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm.Departments = await GetDepartmentsSelectListAsync(ct);
                return View(vm);
            }

            var employee = _mapper.Map<Employee>(vm);

            try
            {
                await _employeeServices.UpdateAsync(employee, ct);
                return RedirectToAction(nameof(Index));
            }
            catch (BusinessRuleException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                vm.Departments = await GetDepartmentsSelectListAsync(ct);
                return View(vm);
            }
        }

        #endregion

        #region Employee Delete

        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var employee = await _employeeServices.GetByIdAsync(id, ct);
            if (employee is null)
                return NotFound();

            var vm = _mapper.Map<EmployeeDetailsVM>(employee);
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var employee = await _employeeServices.GetByIdAsync(id, ct);
            if (employee is null)
                return NotFound();

            try
            {
                await _employeeServices.DeleteAsync(employee, ct);
            }
            catch (BusinessRuleException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Helper
        private async Task<IEnumerable<SelectListItem>> GetDepartmentsSelectListAsync(CancellationToken ct)
        {
            var departments = await _departmentServices.GetAllAsync(ct);
            return departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
        } 
        #endregion

    }
}