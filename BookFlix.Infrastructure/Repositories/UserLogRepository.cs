using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    internal class UserLogRepository : IUserLogRepository
    {
        private readonly AppDbContext _context;
        public UserLogRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<UserLog> AddAsync(UserLog entity)
        {
            await _context.UserLogs.AddAsync(entity);
            return entity;
        }

        public async Task<IReadOnlyCollection<UserLog>> GetLogsByUserIDAsync(Guid userID)
            => await _context.UserLogs.AsNoTracking()
                                      .Where(ul => ul.UserID == userID)
                                      .OrderByDescending(ul => ul.Timestamp)
                                      .ToListAsync();
    }
}
