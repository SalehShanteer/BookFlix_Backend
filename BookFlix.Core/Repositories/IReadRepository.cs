namespace BookFlix.Core.Repositories
{
    public interface IReadRepository<T> where T : class
    {
        Task<T> GetByIDAsync(Guid id);
        Task<T> GetByIDForUpdateAsync(Guid id);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<bool> IsExistByIDAsync(Guid id);
    }
}
