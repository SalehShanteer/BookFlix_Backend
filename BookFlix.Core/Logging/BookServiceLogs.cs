using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Logging
{
    public static partial class BookServiceLogs
    {
        [LoggerMessage(EventId = 1001, Level = LogLevel.Information, Message = "Executing AddBookAsync for Title '{Title}'")]
        public static partial void LogAddingBook(this ILogger logger, string title);

        [LoggerMessage(EventId = 1002, Level = LogLevel.Warning, Message = "AddBookAsync failed for Title '{Title}'. Error: {ErrorCode}")]
        public static partial void LogAddBookFailed(this ILogger logger, string title, string errorCode);

        [LoggerMessage(EventId = 1003, Level = LogLevel.Information, Message = "Successfully added Book '{Title}' with ID {BookId}")]
        public static partial void LogAddBookSuccess(this ILogger logger, string title, Guid bookId);

        [LoggerMessage(EventId = 1004, Level = LogLevel.Information, Message = "Executing GetAllBooksAsync")]
        public static partial void LogGetAllBooksExecuting(this ILogger logger);

        [LoggerMessage(EventId = 1005, Level = LogLevel.Information, Message = "Retrieved {Count} books")]
        public static partial void LogGetAllBooksRetrieved(this ILogger logger, int count);

        [LoggerMessage(EventId = 1006, Level = LogLevel.Information, Message = "Executing GetBooksByAuthorAsync for AuthorID {AuthorId}")]
        public static partial void LogGetBooksByAuthorExecuting(this ILogger logger, Guid authorId);

        [LoggerMessage(EventId = 1007, Level = LogLevel.Information, Message = "Retrieved {Count} books for AuthorID {AuthorId}")]
        public static partial void LogGetBooksByAuthorRetrieved(this ILogger logger, int count, Guid authorId);

        [LoggerMessage(EventId = 1008, Level = LogLevel.Information, Message = "Executing GetBookByIDAsync for ID {BookId}")]
        public static partial void LogGetBookByIdExecuting(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1009, Level = LogLevel.Warning, Message = "Book with ID {BookId} was not found")]
        public static partial void LogGetBookByIdNotFound(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1010, Level = LogLevel.Information, Message = "Executing GetBookByIDForUpdateAsync for ID {BookId}")]
        public static partial void LogGetBookByIdForUpdateExecuting(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1011, Level = LogLevel.Information, Message = "Executing UpdateBookAsync for BookID {BookId}")]
        public static partial void LogUpdatingBookExecuting(this ILogger logger, Guid? bookId);

        [LoggerMessage(EventId = 1012, Level = LogLevel.Warning, Message = "UpdateBookAsync failed for BookID {BookId}. Error: {ErrorCode}")]
        public static partial void LogUpdateBookFailed(this ILogger logger, Guid? bookId, string errorCode);

        [LoggerMessage(EventId = 1013, Level = LogLevel.Information, Message = "Successfully updated Book {BookId}")]
        public static partial void LogUpdateBookSuccess(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1014, Level = LogLevel.Information, Message = "Executing DeleteBookAsync for BookID {BookId}")]
        public static partial void LogDeletingBookExecuting(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1015, Level = LogLevel.Warning, Message = "DeleteBookAsync failed for BookID {BookId}. Error: {ErrorCode}")]
        public static partial void LogDeleteBookFailed(this ILogger logger, Guid bookId, string errorCode);

        [LoggerMessage(EventId = 1016, Level = LogLevel.Information, Message = "Successfully deleted BookID {BookId}")]
        public static partial void LogDeleteBookSuccess(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1017, Level = LogLevel.Information, Message = "Executing GetBookByIsbnAsync for ISBN '{Isbn}'")]
        public static partial void LogGetBookByIsbnExecuting(this ILogger logger, string isbn);

        [LoggerMessage(EventId = 1018, Level = LogLevel.Information, Message = "Executing GetBookFilePathAsync for BookID {BookId}")]
        public static partial void LogGetBookFilePathExecuting(this ILogger logger, Guid bookId);

        [LoggerMessage(EventId = 1019, Level = LogLevel.Warning, Message = "GetBookFilePathAsync failed for BookID {BookId}. Error: {ErrorCode}")]
        public static partial void LogGetBookFilePathFailed(this ILogger logger, Guid bookId, string errorCode);
    }
}
