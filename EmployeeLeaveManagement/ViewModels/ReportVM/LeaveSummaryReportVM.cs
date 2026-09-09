namespace EmployeeLeaveManagementWeb.ViewModels.ReportVM
{
    public class LeaveSummaryReportVM
    {
        public string EmployeeName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public int TotalRequests { get; set; }
        public int ApprovedRequests { get; set; }
        public int RejectedRequests { get; set; }
        public int PendingRequests { get; set; }
        public int TotalApprovedDays { get; set; }
    }
}