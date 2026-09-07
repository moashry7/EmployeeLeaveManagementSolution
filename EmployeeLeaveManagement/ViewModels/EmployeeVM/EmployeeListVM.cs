namespace EmployeeLeaveManagementWeb.ViewModels.EmployeeVM
{
    public class EmployeeListVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string Status { get; set; } = null!;


    }
}
