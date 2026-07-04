using BookFlix.Core.Abstractions;
using BookFlix.Core.Repositories;
using BookFlix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookFlix.Infrastructure.Repositories
{
    public class EntityRepository<T> : TransactionRepository, IEntityRepository<T> where T : class, IEntity
    {
        protected readonly AppDbContext Context;

        public EntityRepository(AppDbContext context) : base(context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public virtual async Task<T> GetByIDAsync(Guid id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByIDForUpdateAsync(Guid id)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(e => e.ID == id);
        }

        public virtual async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await Context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<bool> IsExistByIDAsync(Guid id)
        {
            return await Context.Set<T>().AsNoTracking().AnyAsync(e => e.ID == id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await GetByIDAsync(id);
            if (entity is null) return false;

            Context.Set<T>().Remove(entity);
            return true;
        }

        public virtual async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
