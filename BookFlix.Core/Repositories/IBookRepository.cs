using BookFlix.Core.Models;

namespace BookFlix.Core.Repositories
{
    public interface IBookRepository : IEntityfileRepository<Book>
    {
        Task<IReadOnlyCollection<Book>> GetByAuthorIDAsync(Guid authorID);
        Task<Book> GetByISBNAsync(string isbn);
        Task<bool> IsExistByIsbnAsync(string isbn);
        Task<bool> IsExistByIsbnAsync(Guid id, string isbn);
    }
}
