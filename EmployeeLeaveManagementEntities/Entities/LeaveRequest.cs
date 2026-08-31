using EmployeeLeaveManagementEntities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementEntities.Entities
{
    public class LeaveRequest
    {

        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveRequestStatus Status { get; set; }
        public int ApprovedById { get; set; }
       

    }

}