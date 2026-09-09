namespace EmployeeLeaveManagementWeb.ViewModels.DepartmentVM
{
    public class DepartmentDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Budget { get; set; }
        public string? ManagerName { get; set; }
        public List<string> EmployeeNames { get; set; } = new();
    }
}
