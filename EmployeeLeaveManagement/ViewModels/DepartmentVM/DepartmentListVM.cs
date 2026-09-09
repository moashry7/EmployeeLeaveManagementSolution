namespace EmployeeLeaveManagementWeb.ViewModels.DepartmentVM
{
    public class DepartmentListVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Budget { get; set; }
        public int EmployeeCount { get; set; }
        public string? ManagerName { get; set; }
    }
}
