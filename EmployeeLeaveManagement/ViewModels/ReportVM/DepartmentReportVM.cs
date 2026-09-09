namespace EmployeeLeaveManagementWeb.ViewModels.ReportVM
{
    public class DepartmentReportVM
    {
        public string Name { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public decimal Budget { get; set; }
        public decimal TotalSalaries { get; set; }
        public decimal RemainingBudget => Budget - TotalSalaries;
        public int UsagePercent => Budget == 0 ? 0 : (int)Math.Min(100, (TotalSalaries / Budget) * 100);
    }
}