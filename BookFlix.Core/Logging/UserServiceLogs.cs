using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Logging
{
    public static partial class UserServiceLogs
    {
        [LoggerMessage(EventId = 2001, Level = LogLevel.Information, Message = "Executing AddUserAsUserAsync for Email '{Email}'")]
        public static partial void LogAddUserAsUserExecuting(this ILogger logger, string email);

        [LoggerMessage(EventId = 2002, Level = LogLevel.Warning, Message = "AddUserAsUserAsync failed for Email '{Email}'. Error: {ErrorCode}")]
        public static partial void LogAddUserAsUserFailed(this ILogger logger, string email, string errorCode);

        [LoggerMessage(EventId = 2003, Level = LogLevel.Information, Message = "Successfully registered user '{Email}' with ID {UserId}")]
        public static partial void LogAddUserAsUserSuccess(this ILogger logger, string email, Guid userId);

        [LoggerMessage(EventId = 2004, Level = LogLevel.Information, Message = "Executing AddUserAsAdminAsync for Email '{Email}'")]
        public static partial void LogAddUserAsAdminExecuting(this ILogger logger, string email);

        [LoggerMessage(EventId = 2005, Level = LogLevel.Warning, Message = "AddUserAsAdminAsync failed for Email '{Email}'. Error: {ErrorCode}")]
        public static partial void LogAddUserAsAdminFailed(this ILogger logger, string email, string errorCode);

        [LoggerMessage(EventId = 2006, Level = LogLevel.Information, Message = "Successfully registered Admin user '{Email}' with ID {UserId}")]
        public static partial void LogAddUserAsAdminSuccess(this ILogger logger, string email, Guid userId);

        [LoggerMessage(EventId = 2007, Level = LogLevel.Information, Message = "Executing GetUserByIDAsync for UserID {UserId}")]
        public static partial void LogGetUserByIdExecuting(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2008, Level = LogLevel.Warning, Message = "GetUserByIDAsync failed for UserID {UserId}. Error: {ErrorCode}")]
        public static partial void LogGetUserByIdFailed(this ILogger logger, Guid userId, string errorCode);

        [LoggerMessage(EventId = 2009, Level = LogLevel.Information, Message = "Executing GetUserByRefreshTokenAsync")]
        public static partial void LogGetUserByRefreshTokenExecuting(this ILogger logger);

        [LoggerMessage(EventId = 2010, Level = LogLevel.Information, Message = "Executing UpdateUserPasswordAsync for UserID {UserId}")]
        public static partial void LogUpdateUserPasswordExecuting(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2011, Level = LogLevel.Warning, Message = "UpdateUserPasswordAsync failed for UserID {UserId}. Error: {ErrorCode}")]
        public static partial void LogUpdateUserPasswordFailed(this ILogger logger, Guid userId, string errorCode);

        [LoggerMessage(EventId = 2012, Level = LogLevel.Information, Message = "Successfully updated password for UserID {UserId}")]
        public static partial void LogUpdateUserPasswordSuccess(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2013, Level = LogLevel.Information, Message = "Executing UpdateUserUsernameAsync for UserID {UserId}")]
        public static partial void LogUpdateUserUsernameExecuting(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2014, Level = LogLevel.Warning, Message = "UpdateUserUsernameAsync failed for UserID {UserId}. Error: {ErrorCode}")]
        public static partial void LogUpdateUserUsernameFailed(this ILogger logger, Guid userId, string errorCode);

        [LoggerMessage(EventId = 2015, Level = LogLevel.Information, Message = "Executing UpdateUserEmailAsync for UserID {UserId}")]
        public static partial void LogUpdateUserEmailExecuting(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2016, Level = LogLevel.Warning, Message = "UpdateUserEmailAsync failed for UserID {UserId}. Error: {ErrorCode}")]
        public static partial void LogUpdateUserEmailFailed(this ILogger logger, Guid userId, string errorCode);

        [LoggerMessage(EventId = 2017, Level = LogLevel.Information, Message = "Executing UpdateUserRefreshTokenAsync")]
        public static partial void LogUpdateUserRefreshTokenExecuting(this ILogger logger);

        [LoggerMessage(EventId = 2018, Level = LogLevel.Warning, Message = "UpdateUserRefreshTokenAsync failed. Error: {ErrorCode}")]
        public static partial void LogUpdateUserRefreshTokenFailed(this ILogger logger, string errorCode);

        [LoggerMessage(EventId = 2019, Level = LogLevel.Information, Message = "Executing RevokeUserRefreshTokenAsync")]
        public static partial void LogRevokeUserRefreshTokenExecuting(this ILogger logger);

        [LoggerMessage(EventId = 2020, Level = LogLevel.Information, Message = "Executing GetAllUsersAsync")]
        public static partial void LogGetAllUsersExecuting(this ILogger logger);

        [LoggerMessage(EventId = 2021, Level = LogLevel.Information, Message = "Executing GetUserProfilePathAsync for UserID {UserId}")]
        public static partial void LogGetUserProfilePathExecuting(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2022, Level = LogLevel.Information, Message = "Executing UploadProfileImageAsync for UserID {UserId}")]
        public static partial void LogUploadProfileImageExecuting(this ILogger logger, Guid userId);

        [LoggerMessage(EventId = 2023, Level = LogLevel.Warning, Message = "UploadProfileImageAsync failed for UserID {UserId}. Error: {ErrorCode}")]
        public static partial void LogUploadProfileImageFailed(this ILogger logger, Guid userId, string errorCode);
    }
}
