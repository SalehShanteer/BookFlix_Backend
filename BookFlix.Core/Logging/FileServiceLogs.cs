using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Logging
{
    public static partial class FileServiceLogs
    {
        [LoggerMessage(EventId = 4001, Level = LogLevel.Warning, Message = "Get file stream failed for {EntityType} with File ID '{FileId}'. Error: {ErrorCode}")]
        public static partial void LogGetFileStreamFailed(this ILogger logger, string entityType, Guid fileId, string errorCode);
        
        [LoggerMessage(EventId = 4002, Level = LogLevel.Warning, Message = "File validation failed for {EntityType}. Error: {ErrorCode}")]
        public static partial void LogFileValidationFailed(this ILogger logger, string entityType, string errorCode);

        [LoggerMessage(EventId = 4003, Level = LogLevel.Information, Message = "Uploading file for {EntityType} with ID '{EntityId}'")]
        public static partial void LogUploadFileExecuting(this ILogger logger, string entityType, Guid entityId);

        [LoggerMessage(EventId = 4004, Level = LogLevel.Warning, Message = "Upload file failed for {EntityType} with ID '{EntityId}'. Error: {ErrorCode}")]
        public static partial void LogUploadFileFailed(this ILogger logger, string entityType, Guid entityId, string errorCode);

        [LoggerMessage(EventId = 4005, Level = LogLevel.Information, Message = "File uploaded successfully for {EntityType} with ID '{EntityId}'. Uploaded File ID: '{FileId}'")]
        public static partial void LogUploadFileSuccess(this ILogger logger, string entityType, Guid entityId, Guid fileId);
    }
}
