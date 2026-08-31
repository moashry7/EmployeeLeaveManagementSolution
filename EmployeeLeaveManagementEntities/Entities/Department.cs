namespace EmployeeLeaveManagementEntities.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Budget { get; set; }
        public int ManagerId { get; set; } 

        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
