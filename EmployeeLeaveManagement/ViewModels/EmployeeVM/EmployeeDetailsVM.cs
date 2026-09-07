namespace EmployeeLeaveManagementWeb.ViewModels.EmployeeVM
{
    public class EmployeeDetailsVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Salary { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public string Status { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    }
}
