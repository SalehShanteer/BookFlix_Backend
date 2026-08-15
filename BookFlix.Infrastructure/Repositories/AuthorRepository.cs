using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    internal class AuthorRepository : EntityRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Author> GetByIDWithBooksAsync(Guid id)
            => await Context.Authors.AsNoTracking().Include(a => a.Books).FirstOrDefaultAsync(a => a.ID == id);
    }
}
