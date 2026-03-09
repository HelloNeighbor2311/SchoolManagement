using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class TeacherConfiguration : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.Property(p => p.Speciality).HasMaxLength(100);
            builder.ToTable(p => p.HasCheckConstraint("CK_Teacher_Speciality", "UserType != 'Teacher' OR Speciality IS NOT NULL"));
            builder.ToTable(p => p.HasCheckConstraint("CK_Teacher_NoEnrollYear", "UserType != 'Teacher' OR EnrollYear IS NULL"));
        }
    }
}
