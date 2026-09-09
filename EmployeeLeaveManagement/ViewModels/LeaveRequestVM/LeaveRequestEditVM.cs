using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeLeaveManagementWeb.ViewModels.LeaveRequestVM
{
    public class LeaveRequestEditVM
    {
        public int Id { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public int LeaveTypeId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<SelectListItem> Employees { get; set; } = new();
        public List<SelectListItem> LeaveTypes { get; set; } = new();
    }
}
