using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementWeb.ViewModels.LeaveTypeVM
{
    public class LeaveTypeCreateVM
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, Range(1, 365)]
        public int DaysAllowedPerYear { get; set; }
    }
}