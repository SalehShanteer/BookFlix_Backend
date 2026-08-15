using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    internal class RoleRepository : EntityRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role> GetByNameAsync(string name)
            => await Context.Roles.FirstOrDefaultAsync(r => r.Name == name);
    }
}
