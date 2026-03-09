using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasKey(p => p.GradeId);
            builder.HasOne(p => p.Enrollment).WithMany(p => p.Grade).HasForeignKey(p => p.EnrollmentId);
            builder.Property(p => p.FirstGrade).HasDefaultValue(false);
            builder.Property(p => p.SecondGrade).HasDefaultValue(false);
            builder.Property(p => p.FinalGrade).HasDefaultValue(false);

        }
    }
}
