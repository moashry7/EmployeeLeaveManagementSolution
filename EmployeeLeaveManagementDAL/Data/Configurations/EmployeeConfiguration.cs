using EmployeeLeaveManagementEntities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace EmployeeLeaveManagementDAL.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.FullName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Email)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasIndex(e => e.Email)
                   .IsUnique();

            builder.Property(e => e.Salary)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Phone)
                   .IsRequired()
                   .HasMaxLength(11);

            builder.HasIndex(e => e.Phone)
                  .IsUnique();

            builder.Property(e => e.JoinDate)
                   .IsRequired();

            builder.HasOne(e => e.Department)
                   .WithMany(d => d.Employees)
                   .HasForeignKey(e => e.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
