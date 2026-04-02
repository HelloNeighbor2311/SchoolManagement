using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(u => u.RefreshTokenId);
            builder.Property(u => u.Token).IsRequired().HasMaxLength(500);
            builder.HasIndex(u => u.Token).IsUnique();
            builder.Property(r => r.IsRevoked).HasDefaultValue(false);
            builder.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
