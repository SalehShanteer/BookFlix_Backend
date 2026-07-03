using BookFlix.Core.Abstractions;

namespace BookFlix.Core.Repositories
{
    public interface IEntityRepository<T> : IReadRepository<T>, IWriteRepository<T> where T : class, IEntity
    {
    }
}
