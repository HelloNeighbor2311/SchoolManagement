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
            builder.HasOne(p => p.Enrollment).WithOne(p => p.Grade).
                HasForeignKey<Grade>(p => p.EnrollmentId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
