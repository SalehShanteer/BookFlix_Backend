using BookFlix.Core.Abstractions;

namespace BookFlix.Core.Repositories
{
    public interface IEntityfileRepository<T> : IEntityRepository<T>, ITransactionRepository, IFileRepository where T : class, IEntityFile
    {
    }
}
