using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class UserRepository : EntityRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> UpdateFileLocationAsync(Guid id, string fileLocation)
        {
            var user = await Context.Users.FindAsync(id);
            if (user is null) return false;

            user.FileLocation = fileLocation;
            user.UpdatedAt = DateTime.UtcNow;
            return true;
        }

        public async Task<User> GetByEmailAsync(string email)
            => await Context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> GetByIDWithRelationsAsync(Guid id)
            => await Context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .Include(u => u.RefreshTokens)
                .Include(u => u.Reviews)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.ID == id);

        public async Task<bool> IsEmailExistAsync(string email)
            => await Context.Users.AsNoTracking().AnyAsync(u => u.Email == email);

        public async Task<bool> IsUsernameExistAsync(string username)
            => await Context.Users.AsNoTracking().AnyAsync(u => u.Username == username);
    }
}