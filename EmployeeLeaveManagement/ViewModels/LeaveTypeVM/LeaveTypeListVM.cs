namespace EmployeeLeaveManagementWeb.ViewModels.LeaveTypeVM
{
    public class LeaveTypeListVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int DaysAllowedPerYear { get; set; }
        public int LeaveRequestCount { get; set; }
    }
}