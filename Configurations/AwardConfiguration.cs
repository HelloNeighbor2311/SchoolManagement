using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class AwardConfiguration : IEntityTypeConfiguration<Award>
    {
        public void Configure(EntityTypeBuilder<Award> builder)
        {
            builder.HasKey(p => p.AwardId);
            builder.Property(p => p.status).HasConversion<string>().HasMaxLength(20);
            builder.ToTable(p => p.HasCheckConstraint("CK_Award_Status", "Status IN ('Approved','Rejected','Pending')"));
            builder.Property(p => p.RequireApproval).IsRequired();
            builder.Property(p => p.ExpiredDate).IsRequired();

            builder.HasOne(p => p.Gpa).WithMany(p => p.Award).HasForeignKey(p => p.GpaId);
            builder.HasOne(p => p.Student).WithMany(p => p.Award).HasForeignKey(p => p.StudentId).OnDelete(DeleteBehavior.Restrict);
            
        }
    }
}
