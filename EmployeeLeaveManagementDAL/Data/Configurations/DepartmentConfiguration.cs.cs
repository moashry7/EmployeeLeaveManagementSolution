using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementDAL.Data.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(d => d.Budget)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(d => d.Manager)
                   .WithMany()
                   .HasForeignKey(d => d.ManagerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(d => d.Employees)
                   .WithOne(e => e.Department)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
