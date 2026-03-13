using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(p => p.CourseId);
            builder.Property(p => p.CourseName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired();

            builder.HasIndex(p => p.CourseName).IsUnique();
        }
    }
}
