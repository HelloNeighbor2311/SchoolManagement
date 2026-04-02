using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(p => p.EnrollmentId);
            builder.HasOne(p => p.Student).WithMany(p => p.Enrollment).HasForeignKey(p => p.StudentId);
            builder.HasOne(p => p.CourseSemester).WithMany(p => p.Enrollment).HasForeignKey(p => p.CourseSemesterId);
            builder.HasIndex(p => new { p.StudentId, p.CourseSemesterId }).IsUnique();
        }
    }
}
