using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveManagementWeb.ViewModels.LeaveTypeVM
{
    public class LeaveTypeEditVM
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, Range(1, 365)]
        public int DaysAllowedPerYear { get; set; }
    }
}