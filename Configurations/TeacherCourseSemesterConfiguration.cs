using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class TeacherCourseSemesterConfiguration : IEntityTypeConfiguration<TeacherCourseSemester>
    {
        public void Configure(EntityTypeBuilder<TeacherCourseSemester> builder)
        {
            builder.HasKey(p => p.TeacherCourseSemesterId);
            builder.HasOne(p => p.CourseSemester).WithMany(p => p.TeacherCourseSemester).HasForeignKey(p => p.CourseSemesterId);
            builder.HasOne(p => p.Teacher).WithMany(p => p.TeacherCourses).HasForeignKey(p => p.TeacherId);

        }
    }
}
