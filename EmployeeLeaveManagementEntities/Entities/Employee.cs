using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementEntities.Entities
{
    public class Employee
    {

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public decimal Salary { get; set; }
        public string Phone { get; set; } = null!;
        public DateTime JoinDate { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public List<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    }
}
