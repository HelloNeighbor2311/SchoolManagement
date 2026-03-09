using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(p => p.EnrollYear).HasMaxLength(20);
            builder.ToTable(p => p.HasCheckConstraint("CK_Student_EnrollYear", 
                "UserType != 'Student' OR EnrollYear IS NOT NULL"));
            builder.ToTable(p => p.HasCheckConstraint("CK_Student_NoSpeciality", 
                "UserType != 'Student' OR Speciality IS NULL"));
        }
    }
}
