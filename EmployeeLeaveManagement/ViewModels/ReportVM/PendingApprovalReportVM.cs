namespace EmployeeLeaveManagementWeb.ViewModels.ReportVM
{
    public class PendingApprovalReportVM
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string LeaveTypeName { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysWaiting { get; set; }
    }
}