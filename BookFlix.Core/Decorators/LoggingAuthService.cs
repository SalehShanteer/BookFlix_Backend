using BookFlix.Core.Logging;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Decorators
{
    public class LoggingAuthService : IAuthService
    {
        private readonly IAuthService _inner;
        private readonly ILogger<LoggingAuthService> _logger;

        public LoggingAuthService(IAuthService inner, ILogger<LoggingAuthService> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<Result<(string AccessToken, string RefreshToken)>> LoginAsync(string email, string password, string ipAddress)
        {
            _logger.LogLoginExecuting(email, ipAddress);

            var result = await _inner.LoginAsync(email, password, ipAddress);

            if (result.IsFailure)
            {
                _logger.LogLoginFailed(email, result.Error.Key);
            }
            else
            {
                _logger.LogLoginSuccess(email, ipAddress);
            }

            return result;
        }
    }
}
