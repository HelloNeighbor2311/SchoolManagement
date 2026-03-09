using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class CourseSemesterConfiguration : IEntityTypeConfiguration<CourseSemester>
    {
        public void Configure(EntityTypeBuilder<CourseSemester> builder)
        {
            builder.HasKey(p => p.CourseSemesterId);

            builder.HasOne(p => p.Course).WithMany(p => p.CourseSemester).HasForeignKey(p => p.CourseId);
            builder.HasOne(p => p.Semester).WithMany(p => p.CourseSemester).HasForeignKey(p => p.SemesterId);
        }
    }
}
