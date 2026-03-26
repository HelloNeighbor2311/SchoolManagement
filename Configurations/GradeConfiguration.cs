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
            builder.Property(g => g.FirstGrade)
               .IsRequired(false);
            builder.Property(g => g.SecondGrade)
                   .IsRequired(false);
            builder.Property(g => g.FinalGrade)
                   .IsRequired(false);
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Grades_FirstGrade",
                    "FirstGrade IS NULL OR (FirstGrade >= 0 AND FirstGrade <= 10)");

                t.HasCheckConstraint("CK_Grade_SecondGrade",
                    "SecondGrade IS NULL OR (SecondGrade >= 0 AND SecondGrade <= 10)");

                t.HasCheckConstraint("CK_Grade_FinalGrade",
                    "FinalGrade IS NULL OR (FinalGrade >= 0 AND FinalGrade <= 10)");
            });
            builder.HasOne(p => p.Enrollment).WithOne(p => p.Grade).
                HasForeignKey<Grade>(p => p.EnrollmentId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
