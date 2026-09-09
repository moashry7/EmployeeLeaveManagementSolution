using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeLeaveManagementWeb.ViewModels.LeaveRequestVM
{
    public class LeaveRequestCreateVM
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public List<SelectListItem> Employees { get; set; } = new();
        public List<SelectListItem> LeaveTypes { get; set; } = new();
    }
}