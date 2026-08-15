using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    internal class GenreRepository : IGenreRepository
    {
        private readonly AppDbContext _context;

        public GenreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyCollection<Genre>> GetAllAsync()
        {
            return await _context.Genres.AsNoTracking().ToListAsync();
        }

        public async Task<Genre> GetByIDAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }
    }
}
