using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementDAL.Data.Configurations
{
    internal class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.HasKey(lr => lr.Id);

            builder.Property(lr => lr.StartDate).IsRequired();

            builder.Property(lr => lr.EndDate).IsRequired();

            builder.Property(lr => lr.Status).IsRequired();

            builder.HasOne(lr => lr.Employee)
                   .WithMany(lr => lr.LeaveRequests)
                   .HasForeignKey(lr => lr.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lr => lr.LeaveType)
                   .WithMany(lt => lt.LeaveRequests)
                   .HasForeignKey(lr => lr.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lr => lr.ApprovedBy)
                   .WithMany()
                   .HasForeignKey(lr => lr.ApprovedById)
                   .OnDelete(DeleteBehavior.Restrict);



        }
    }
}
