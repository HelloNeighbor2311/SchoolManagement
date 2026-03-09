using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class GpaConfiguration : IEntityTypeConfiguration<Gpa>
    {
        public void Configure(EntityTypeBuilder<Gpa> builder)
        {
            builder.HasKey(p => p.GPAId);
            builder.HasOne(p => p.Student).WithMany(p => p.Gpa).HasForeignKey(p => p.StudentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.Semester).WithOne(p => p.Gpa).HasForeignKey<Gpa>(p => p.SemesterId);
            builder.Property(p => p.gpa).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.rank).IsRequired();
            builder.ToTable(p => p.HasCheckConstraint("CK_Gpa_rank", "rank In ('Excellent','Good','Average','Bad')"));
        }
    }
}
