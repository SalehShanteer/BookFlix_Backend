using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IUserRepository : IEntityfileRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIDWithRelationsAsync(Guid id);
        Task<bool> IsEmailExistAsync(string email);
        Task<bool> IsUsernameExistAsync(string username);
    }
}
