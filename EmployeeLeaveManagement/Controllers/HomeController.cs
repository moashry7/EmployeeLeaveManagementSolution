using AutoMapper;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementEntities.Enums;
using EmployeeLeaveManagementWeb.ViewModels.HomeVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagementWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IDepartmentServices _departmentServices;
        private readonly ILeaveRequestServices _leaveRequestServices;

        public HomeController(
            IEmployeeServices employeeServices,
            IDepartmentServices departmentServices,
            ILeaveRequestServices leaveRequestServices)
        {
            _employeeServices = employeeServices;
            _departmentServices = departmentServices;
            _leaveRequestServices = leaveRequestServices;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var employees = (await _employeeServices.GetAllAsync(ct)).ToList();
            var departments = (await _departmentServices.GetAllAsync(ct)).ToList();
            var leaveRequests = (await _leaveRequestServices.GetAllAsync(ct)).ToList();

            var vm = new DashboardVM
            {
                TotalEmployees = employees.Count,
                ActiveEmployees = employees.Count(e => e.Status == EmployeeStatus.Active),
                InactiveEmployees = employees.Count(e => e.Status == EmployeeStatus.Inactive),

                TotalDepartments = departments.Count,

                PendingLeaveRequests = leaveRequests.Count(r => r.Status == LeaveRequestStatus.Pending),
                ApprovedLeaveRequests = leaveRequests.Count(r => r.Status == LeaveRequestStatus.Approved),
                RejectedLeaveRequests = leaveRequests.Count(r => r.Status == LeaveRequestStatus.Rejected),

                RecentLeaveRequests = leaveRequests
                    .OrderByDescending(r => r.StartDate)
                    .Take(5)
                    .Select(r => new RecentLeaveRequestVM
                    {
                        EmployeeName = r.Employee.FullName,
                        LeaveTypeName = r.LeaveType.Name,
                        StartDate = r.StartDate,
                        EndDate = r.EndDate,
                        Status = r.Status.ToString()
                    })
                    .ToList(),

                DepartmentBudgets = departments
                    .Select(d => new DepartmentBudgetVM
                    {
                        Name = d.Name,
                        Budget = d.Budget,
                        UsedSalaries = d.Employees.Sum(e => e.Salary)
                    })
                    .OrderByDescending(d => d.UsagePercent)
                    .ToList()
            };

            return View(vm);
        }

        public IActionResult Privacy() => View();
    }
}