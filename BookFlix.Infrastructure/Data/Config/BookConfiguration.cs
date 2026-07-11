using BookFlix.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookFlix.Infrastructure.Data.Config
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("Books");
            builder.HasKey(b => b.ID);
            builder.HasIndex(b => b.ISBN)
                .IsUnique();

            builder.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(150);
            builder.Property(b => b.Description)
                .HasMaxLength(1000);
            builder.Property(b => b.ISBN)
                .HasMaxLength(20);
            builder.Property(b => b.CoverImageUrl)
                .HasMaxLength(500);
            builder.Property(b => b.PublicationDate)
                .HasColumnType("datetime");
            builder.Property(b => b.Publisher)
                .HasMaxLength(100);
            builder.Property(b => b.PageCount)
                .IsRequired(false);
            builder.Property(b => b.AverageRating)
                .HasColumnType("decimal(3, 2)");
            builder.Property(b => b.IsAvailable)
                .IsRequired();
            builder.Property(b => b.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(b => b.UpdatedAt)
                .IsRequired(false);

            builder.Property(b => b.FileID)
                .IsRequired(false);

            builder.HasOne(b => b.File)
                .WithMany()
                .HasForeignKey(b => b.FileID)
                .OnDelete(DeleteBehavior.SetNull);

            // Relationships
            builder.HasMany(b => b.Authors)
                .WithMany(a => a.Books)
                .UsingEntity<BookAuthor>()
                .HasKey(ba => new { ba.BookID, ba.AuthorID });

            builder.HasMany(b => b.Reviews)
                .WithOne(r => r.Book)
                .HasForeignKey(r => r.BookID);

            builder.HasMany(b => b.Genres)
                .WithMany(g => g.Books)
                .UsingEntity<BookGenre>()
                .HasKey(bg => new { bg.BookID, bg.GenreID });

            // Seed data
            builder.HasData(SeedData.LoadBookData());
        }
    }
}


