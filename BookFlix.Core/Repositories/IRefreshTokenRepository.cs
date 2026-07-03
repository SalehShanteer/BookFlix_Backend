using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> AddAsync(RefreshToken entity);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task SaveChangesAsync();
    }
}
