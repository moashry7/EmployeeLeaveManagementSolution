using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementWeb.ViewModels.DepartmentVM
{
    public class DepartmentCreateVM
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, Range(0, double.MaxValue)]
        public decimal Budget { get; set; }
    }
}
