using BookFlix.Core.Logging;
using BookFlix.Core.Models;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Decorators
{
    public class LoggingBookService : IBookService
    {
        private readonly IBookService _inner;
        private readonly ILogger<LoggingBookService> _logger;

        public LoggingBookService(IBookService inner, ILogger<LoggingBookService> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<Result<Book>> AddBookAsync(Book book)
        {
            _logger.LogAddingBook(book?.Title);
            var result = await _inner.AddBookAsync(book);
            if (result.IsFailure)
            {
                _logger.LogAddBookFailed(book?.Title, result.Error.Key);
            }
            else
            {
                _logger.LogAddBookSuccess(result.Value.Title, result.Value.ID);
            }
            return result;
        }

        public async Task<IReadOnlyCollection<Book>> GetAllBooksAsync()
        {
            _logger.LogGetAllBooksExecuting();
            var books = await _inner.GetAllBooksAsync();
            _logger.LogGetAllBooksRetrieved(books.Count);
            return books;
        }

        public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(Guid authorID)
        {
            _logger.LogGetBooksByAuthorExecuting(authorID);
            var books = await _inner.GetBooksByAuthorAsync(authorID);
            _logger.LogGetBooksByAuthorRetrieved(books.Count, authorID);
            return books;
        }

        public async Task<Book> GetBookByIDAsync(Guid id)
        {
            _logger.LogGetBookByIdExecuting(id);
            var book = await _inner.GetBookByIDAsync(id);
            if (book is null)
            {
                _logger.LogGetBookByIdNotFound(id);
            }
            return book;
        }

        public async Task<Book> GetBookByIDForUpdateAsync(Guid id)
        {
            _logger.LogGetBookByIdForUpdateExecuting(id);
            return await _inner.GetBookByIDForUpdateAsync(id);
        }

        public async Task<Result<Book>> UpdateBookAsync(Book book)
        {
            _logger.LogUpdatingBookExecuting(book?.ID);
            var result = await _inner.UpdateBookAsync(book);
            if (result.IsFailure)
            {
                _logger.LogUpdateBookFailed(book?.ID, result.Error.Key);
            }
            else
            {
                _logger.LogUpdateBookSuccess(result.Value.ID);
            }
            return result;
        }

        public async Task<Result> DeleteBookAsync(Guid id)
        {
            _logger.LogDeletingBookExecuting(id);
            var result = await _inner.DeleteBookAsync(id);
            if (result.IsFailure)
            {
                _logger.LogDeleteBookFailed(id, result.Error.Key);
            }
            else
            {
                _logger.LogDeleteBookSuccess(id);
            }
            return result;
        }

        public async Task<Book> GetBookByIsbnAsync(string isbn)
        {
            _logger.LogGetBookByIsbnExecuting(isbn);
            return await _inner.GetBookByIsbnAsync(isbn);
        }

        public async Task<Result<string>> GetBookFilePathAsync(Guid bookID)
        {
            _logger.LogGetBookFilePathExecuting(bookID);
            var result = await _inner.GetBookFilePathAsync(bookID);
            if (result.IsFailure)
            {
                _logger.LogGetBookFilePathFailed(bookID, result.Error.Key);
            }
            return result;
        }
    }
}
