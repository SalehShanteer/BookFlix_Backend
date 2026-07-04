using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IRoleRepository : IEntityRepository<Role>
    {        
        Task<Role> GetByNameAsync(string name);
    }
}
