using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<BookService> _logger;

        public BookService(IBookRepository bookRepository, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<Result<Book>> AddBookAsync(Book book)
        {
            var validationResult = await ValidateBookAsync(book, true);
            if (validationResult.IsFailure) return Result.Failure<Book>(validationResult.Error);

            var addedBook = await _bookRepository.AddAsync(book);
            return Result.Success(addedBook);
        }

        public async Task<Result<Book>> UpdateBookAsync(Book updatedBook)
        {
            var validationResult = await ValidateBookAsync(updatedBook, false);
            if (validationResult.IsFailure) return Result.Failure<Book>(validationResult.Error);

            var existingBook = await _bookRepository.GetByIDForUpdateAsync(updatedBook.ID);
            if (existingBook is null)
            {
                _logger.LogWarning("Update failed: Book {BookID} not found.", updatedBook.ID);
                return Result.Failure<Book>(Error.NotFound("BookNotFound"));
            }

            UpdateBookProperties(existingBook, updatedBook);
            await _bookRepository.SaveChangesAsync();

            return Result.Success(existingBook);
        }

        public async Task<Result> DeleteBookAsync(Guid id)
        {
            using var transaction = await _bookRepository.BeginTransactionAsync();
            try
            {
                var book = await _bookRepository.GetByIDAsync(id);
                if (book is null)
                {
                    return Result.Failure(Error.NotFound("BookNotFound"));
                }

                await _bookRepository.DeleteAsync(id);
                await transaction.CommitAsync();

                return Result.Success();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An unexpected error occurred while deleting book {BookID}", id);
                return Result.Failure(Error.Failure("BookDeleteError"));
            }
        }

        public async Task<IReadOnlyCollection<Book>> GetAllBooksAsync() => await _bookRepository.GetAllAsync();
        public async Task<Book> GetBookByIDAsync(Guid id) => await _bookRepository.GetByIDAsync(id);

        public async Task<Book> GetBookByIsbnAsync(string isbn) => await _bookRepository.GetByISBNAsync(isbn);

        public async Task<Book> GetBookByIDForUpdateAsync(Guid id) => await _bookRepository.GetByIDForUpdateAsync(id);

        public async Task<IReadOnlyCollection<Book>> GetBooksByAuthorAsync(Guid authorID) => await _bookRepository.GetByAuthorIDAsync(authorID);

        private async Task<Result> ValidateBookAsync(Book book, bool isNew)
        {
            if (book.PublicationDate > DateTime.UtcNow)
            {
                _logger.LogWarning("Validation failed: Future publication date for book {Title}", book.Title);
                return Result.Failure(Error.Validation("FuturePublicationDate"));
            }

            if (string.IsNullOrWhiteSpace(book.ISBN))
            {
                return Result.Failure(Error.Validation("IsbnRequired"));
            }

            bool isExist = isNew
                ? await _bookRepository.IsExistByIsbnAsync(book.ISBN)
                : await _bookRepository.IsExistByIsbnAsync(book.ID, book.ISBN);

            if (isExist)
            {
                _logger.LogWarning("Validation failed: ISBN {ISBN} already exists.", book.ISBN);
                return Result.Failure(Error.Conflict("DuplicateIsbn"));
            }

            return Result.Success();
        }

        private void UpdateBookProperties(Book existingBook, Book updatedBook)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
            existingBook.ISBN = updatedBook.ISBN;
            existingBook.CoverImageUrl = updatedBook.CoverImageUrl;
            existingBook.PublicationDate = updatedBook.PublicationDate;
            existingBook.Publisher = updatedBook.Publisher;
            existingBook.PageCount = updatedBook.PageCount;
            existingBook.IsAvailable = updatedBook.IsAvailable;
            existingBook.UpdatedAt = DateTime.UtcNow;

            existingBook.Authors.Clear();
            foreach (var author in updatedBook.Authors) existingBook.Authors.Add(author);

            existingBook.Genres.Clear();
            foreach (var genre in updatedBook.Genres) existingBook.Genres.Add(genre);
        }
    }
}