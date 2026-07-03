namespace BookFlix.Core.Repositories
{
    public interface IWriteRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<bool> DeleteAsync(Guid id);
        Task SaveChangesAsync();
    }
}
