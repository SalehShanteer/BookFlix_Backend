using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
    {
        public void Configure(EntityTypeBuilder<UploadedFile> builder)
        {
            builder.ToTable("UploadedFiles");
            builder.HasKey(f => f.ID);
            builder.Property(f => f.FileLocation).IsRequired().HasMaxLength(255);
            builder.Property(f => f.ContentType).IsRequired().HasMaxLength(100);
            builder.Property(f => f.CreatedAt).IsRequired().HasDefaultValueSql("SYSUTCDATETIME()");
            builder.Property(f => f.UpdatedAt).IsRequired(false);
        }
    }
}
