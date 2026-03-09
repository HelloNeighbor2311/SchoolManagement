using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.Models;

namespace SchoolManagement.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(p => p.UserId);

            builder.HasDiscriminator<string>("UserType").
                HasValue<Admin>("Admin").
                HasValue<Student>("Student").
                HasValue<Teacher>("Teacher");

            builder.HasOne(p => p.Role).WithMany(p => p.User).HasForeignKey(p => p.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(p => p.Username).HasMaxLength(100).IsRequired();
            builder.Property(p => p.PasswordHashed).IsRequired();
            builder.Property(p => p.Name).HasMaxLength(100).IsRequired();
            builder.Property(p => p.Email).HasMaxLength(100).IsRequired();
            builder.Property(p => p.CreatedDate).HasDefaultValueSql("GETUTCDATE()");
            builder.Property(p => p.RowVersion).IsRowVersion().IsConcurrencyToken();

            builder.HasIndex(p => p.Username).IsUnique();
            builder.HasIndex(p => p.Email).IsUnique();

        }
    }
}
