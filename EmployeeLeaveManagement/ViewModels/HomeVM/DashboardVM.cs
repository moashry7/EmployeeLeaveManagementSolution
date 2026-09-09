namespace EmployeeLeaveManagementWeb.ViewModels.HomeVM
{
    public class DashboardVM
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int InactiveEmployees { get; set; }

        public int TotalDepartments { get; set; }

        public int PendingLeaveRequests { get; set; }
        public int ApprovedLeaveRequests { get; set; }
        public int RejectedLeaveRequests { get; set; }

        public List<RecentLeaveRequestVM> RecentLeaveRequests { get; set; } = new();
        public List<DepartmentBudgetVM> DepartmentBudgets { get; set; } = new();
    }
}