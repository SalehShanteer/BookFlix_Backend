using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IBookService
    {
        Task<Result<Book>> AddBookAsync(Book book);
        Task<IReadOnlyCollection<Book>> GetAllBooksAsync();
        Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(Guid authorID);
        Task<Book> GetBookByIDAsync(Guid id);
        Task<Book> GetBookByIDForUpdateAsync(Guid id);
        Task<Result<Book>> UpdateBookAsync(Book book);
        Task<Result> DeleteBookAsync(Guid id);
        Task<Book> GetBookByIsbnAsync(string isbn);
        Task<Result<string>> GetBookFilePathAsync(Guid bookID);
        Task<Result<(Stream Stream, string ContentType, string FileName)>> GetBookFileStreamAsync(Guid bookID);
    }
}
