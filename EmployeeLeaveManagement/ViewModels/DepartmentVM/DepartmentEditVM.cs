using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementWeb.ViewModels.DepartmentVM
{
    public class DepartmentEditVM
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = null!;

        [Required, Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        public int? ManagerId { get; set; }

        public IEnumerable<SelectListItem>? Employees { get; set; }
    }
}
