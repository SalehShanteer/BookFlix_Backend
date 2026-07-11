using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.ID);
            builder.HasIndex(u => u.Username)
                .IsUnique();
            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(30);
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(60);
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(u => u.UpdatedAt)
                .IsRequired(false);

            builder.Property(u => u.FileLocation)
                .IsRequired(false)
                .HasMaxLength(50);

            // Relationships
            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity<UserRole>()
                .HasKey(ur => new { ur.UserID, ur.RoleID });

            builder.HasMany(u => u.Reviews)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserID);

            builder.HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserID);

            builder.HasData(SeedData.LoadUsersData());
        }
    }
}
