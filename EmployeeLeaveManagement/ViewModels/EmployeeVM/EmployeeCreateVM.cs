using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementWeb.ViewModels.EmployeeVM
{
    public class EmployeeCreateVM
    {
        [Required(ErrorMessage = "Full Name is required")]
        [MaxLength(100, ErrorMessage = "Full Name cannot exceed 100 characters")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Salary is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Salary")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Join Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date")]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DepartmentId { get; set; }

        public IEnumerable<SelectListItem>? Departments { get; set; }
    }
}

