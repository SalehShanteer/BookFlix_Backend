using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Logging
{
    public static partial class AuthServiceLogs
    {
        [LoggerMessage(EventId = 3001, Level = LogLevel.Information, Message = "Executing LoginAsync for Email '{Email}' from IP {IPAddress}")]
        public static partial void LogLoginExecuting(this ILogger logger, string email, string ipAddress);

        [LoggerMessage(EventId = 3002, Level = LogLevel.Warning, Message = "Login failed for Email '{Email}'. Error: {ErrorCode}")]
        public static partial void LogLoginFailed(this ILogger logger, string email, string errorCode);

        [LoggerMessage(EventId = 3003, Level = LogLevel.Information, Message = "Login successful for Email '{Email}' from IP {IPAddress}")]
        public static partial void LogLoginSuccess(this ILogger logger, string email, string ipAddress);
    }
}
