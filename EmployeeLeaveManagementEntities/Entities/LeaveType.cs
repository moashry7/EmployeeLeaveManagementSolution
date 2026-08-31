using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementEntities.Entities
{
    public class LeaveType
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int DaysAllowedPerYear { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    }
}
