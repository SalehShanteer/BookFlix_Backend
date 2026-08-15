using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    internal class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> AddAsync(RefreshToken entity)
        {
            await _context.RefreshTokens.AddAsync(entity);
            return entity;
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
            => await _context.RefreshTokens.SingleOrDefaultAsync(rt => rt.Token == token);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
