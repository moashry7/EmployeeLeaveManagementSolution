using EmployeeLeaveManagementEntities.Enums;

namespace EmployeeLeaveManagementWeb.ViewModels.LeaveRequestVM
{
    public class LeaveRequestListVM
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveRequestStatus Status { get; set; }
    }
}