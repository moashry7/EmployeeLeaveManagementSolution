using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementDAL.Data.Configurations
{
    internal class LeaveTypeConfiguration : IEntityTypeConfiguration<LeaveType>
    {
        public void Configure(EntityTypeBuilder<LeaveType> builder)
        {
            builder.HasKey(lt => lt.Id);

            builder.Property(lt => lt.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(lt => lt.DaysAllowedPerYear)
                   .IsRequired();

            builder.HasMany(lt => lt.LeaveRequests)
                   .WithOne(lr => lr.LeaveType)
                   .HasForeignKey(lr => lr.LeaveTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
