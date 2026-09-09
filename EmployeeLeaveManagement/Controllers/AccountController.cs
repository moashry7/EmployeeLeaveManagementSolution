using System.Security.Claims;
using EmployeeLeaveManagementBLL.Services.Interfaces;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using EmployeeLeaveManagementEntities.Entities;
using EmployeeLeaveManagementEntities.Enums;
using EmployeeLeaveManagementWeb.ViewModels.AccountVM;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeLeaveManagementWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmployeeServices _employeeServices;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PasswordHasher<Employee> _passwordHasher = new();

        public AccountController(IEmployeeServices employeeServices, IUnitOfWork unitOfWork)
        {
            _employeeServices = employeeServices;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM vm, string? returnUrl, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(vm);

            var employees = await _employeeServices.GetAllAsync(ct);
            var employee = employees.FirstOrDefault(e =>
                e.Email.Equals(vm.Email, StringComparison.OrdinalIgnoreCase));

            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(vm);
            }

            var result = _passwordHasher.VerifyHashedPassword(employee, employee.PasswordHash, vm.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(vm);
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
                new(ClaimTypes.Name, employee.FullName),
                new(ClaimTypes.Email, employee.Email),
                new(ClaimTypes.Role, employee.Role.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                new AuthenticationProperties { IsPersistent = true });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied() => View();
    }
}