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
            builder.HasOne(p => p.Semester).WithMany(p => p.Gpa).HasForeignKey(p => p.SemesterId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.gpa).IsRequired(false);
            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Gpa_gpa",
                    "gpa IS NULL OR (gpa >= 0 AND gpa <= 4)");
            });
            builder.HasIndex(u => new { u.StudentId, u.SemesterId }).IsUnique();
            builder.Property(p => p.rank).HasConversion<string>().HasMaxLength(20); 
            builder.ToTable(p => p.HasCheckConstraint("CK_Gpa_rank", "rank In ('Excellent','Good','Average','Bad')"));
        }
    }
}
