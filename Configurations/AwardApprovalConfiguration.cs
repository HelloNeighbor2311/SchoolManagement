using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class AwardApprovalConfiguration : IEntityTypeConfiguration<AwardApproval>
    {
        public void Configure(EntityTypeBuilder<AwardApproval> builder)
        {
            builder.HasKey(p => p.ApprovalId);
            builder.Property(p => p.decision).HasMaxLength(100).IsRequired();
            builder.ToTable(p=> p.HasCheckConstraint("CK_AwardApproval_decision", "decision IN ('Approve','Reject')"));
            builder.Property(p => p.DecisionDate).HasColumnType("datetime2").IsRequired();

            builder.HasOne(p => p.Teacher).WithMany(p => p.AwardApprovals).HasForeignKey(p => p.TeacherId);
            builder.HasOne(p => p.Award).WithMany(p => p.AwardApprovals).HasForeignKey(p => p.AwardId);
            builder.HasIndex(p => new { p.AwardId, p.TeacherId }).IsUnique();
        }
    }
}
