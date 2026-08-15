using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    internal class BookRepository : EntityRepository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<IReadOnlyCollection<Book>> GetAllAsync()
        => await Context.Books
                .AsSplitQuery()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .AsNoTracking()
                .ToListAsync();

        public async Task<IReadOnlyCollection<Book>> GetByAuthorIDAsync(Guid authorID)
        => await Context.Books
                .AsNoTracking()
                .Where(b => b.Authors.Any(a => a.ID == authorID))
                .ToListAsync();

        public override async Task<Book> GetByIDAsync(Guid id)
        => await Context.Books
                .AsSplitQuery()
                .AsNoTracking()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ID == id);

        public override async Task<Book> GetByIDForUpdateAsync(Guid id)
             => await Context.Books
                .AsSplitQuery()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ID == id);

        public async Task<bool> UpdateFileIDAsync(Guid id, Guid fileId)
        {
            var book = await Context.Books.FindAsync(id);
            if (book is null) return false;

            book.FileID = fileId;
            book.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public override async Task<bool> DeleteAsync(Guid id)
        {
            var book = await Context.Books.FindAsync(id);
            if (book is null) return false;

            Context.Books.Remove(book);
            return true;
        }

        public async Task<Book> GetByISBNAsync(string isbn)
        => await Context.Books
                .AsSplitQuery()
                .AsNoTracking()
                .Include(b => b.Authors)
                .Include(b => b.Genres)
                .FirstOrDefaultAsync(b => b.ISBN == isbn);

        public async Task<bool> IsExistByIsbnAsync(string isbn) => await Context.Books.AsNoTracking().AnyAsync(b => b.ISBN == isbn);

        public async Task<bool> IsExistByIsbnAsync(Guid id, string isbn) => await Context.Books.AsNoTracking().AnyAsync(b => b.ISBN == isbn && b.ID != id);
    }
}