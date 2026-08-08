using BookFlix.Core.Logging;
using BookFlix.Core.Models;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BookFlix.Core.Decorators
{
    public class LoggingUserService : IUserService
    {
        private readonly IUserService _inner;
        private readonly ILogger<LoggingUserService> _logger;

        public LoggingUserService(IUserService inner, ILogger<LoggingUserService> logger)
        {
            _inner = inner;
            _logger = logger;
        }

        public async Task<Result<User>> AddUserAsUserAsync(User user)
        {
            _logger.LogAddUserAsUserExecuting(user?.Email);
            var result = await _inner.AddUserAsUserAsync(user);
            if (result.IsFailure)
            {
                _logger.LogAddUserAsUserFailed(user?.Email, result.Error.Key);
            }
            else
            {
                _logger.LogAddUserAsUserSuccess(result.Value.Email, result.Value.ID);
            }
            return result;
        }

        public async Task<Result<User>> AddUserAsAdminAsync(User user)
        {
            _logger.LogAddUserAsAdminExecuting(user?.Email);
            var result = await _inner.AddUserAsAdminAsync(user);
            if (result.IsFailure)
            {
                _logger.LogAddUserAsAdminFailed(user?.Email, result.Error.Key);
            }
            else
            {
                _logger.LogAddUserAsAdminSuccess(result.Value.Email, result.Value.ID);
            }
            return result;
        }

        public async Task<Result<User>> GetUserByIDAsync(Guid id)
        {
            _logger.LogGetUserByIdExecuting(id);
            var result = await _inner.GetUserByIDAsync(id);
            if (result.IsFailure)
            {
                _logger.LogGetUserByIdFailed(id, result.Error.Key);
            }
            return result;
        }

        public async Task<User> GetUserByRefreshTokenAsync(string token)
        {
            _logger.LogGetUserByRefreshTokenExecuting();
            return await _inner.GetUserByRefreshTokenAsync(token);
        }

        public async Task<Result> UpdateUserPasswordAsync(Guid userID, string oldPassword, string newPassword)
        {
            _logger.LogUpdateUserPasswordExecuting(userID);
            var result = await _inner.UpdateUserPasswordAsync(userID, oldPassword, newPassword);
            if (result.IsFailure)
            {
                _logger.LogUpdateUserPasswordFailed(userID, result.Error.Key);
            }
            else
            {
                _logger.LogUpdateUserPasswordSuccess(userID);
            }
            return result;
        }

        public async Task<Result<User>> UpdateUserUsernameAsync(Guid id, string username)
        {
            _logger.LogUpdateUserUsernameExecuting(id);
            var result = await _inner.UpdateUserUsernameAsync(id, username);
            if (result.IsFailure)
            {
                _logger.LogUpdateUserUsernameFailed(id, result.Error.Key);
            }
            return result;
        }

        public async Task<Result<User>> UpdateUserEmailAsync(Guid id, string email)
        {
            _logger.LogUpdateUserEmailExecuting(id);
            var result = await _inner.UpdateUserEmailAsync(id, email);
            if (result.IsFailure)
            {
                _logger.LogUpdateUserEmailFailed(id, result.Error.Key);
            }
            return result;
        }

        public async Task<Result<(string AccessToken, string RefreshToken)>> UpdateUserRefreshTokenAsync(string refreshToken)
        {
            _logger.LogUpdateUserRefreshTokenExecuting();
            var result = await _inner.UpdateUserRefreshTokenAsync(refreshToken);
            if (result.IsFailure)
            {
                _logger.LogUpdateUserRefreshTokenFailed(result.Error.Key);
            }
            return result;
        }

        public async Task RevokeUserRefreshTokenAsync(string refreshToken)
        {
            _logger.LogRevokeUserRefreshTokenExecuting();
            await _inner.RevokeUserRefreshTokenAsync(refreshToken);
        }

        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync()
        {
            _logger.LogGetAllUsersExecuting();
            return await _inner.GetAllUsersAsync();
        }

        public async Task<Result<string>> GetUserProfilePathAsync(Guid userID)
        {
            _logger.LogGetUserProfilePathExecuting(userID);
            return await _inner.GetUserProfilePathAsync(userID);
        }

        public async Task<Result<Guid>> UploadProfileImageAsync(Guid userID, IFormFile file)
        {
            _logger.LogUploadProfileImageExecuting(userID);
            var result = await _inner.UploadProfileImageAsync(userID, file);
            if (result.IsFailure)
            {
                _logger.LogUploadProfileImageFailed(userID, result.Error.Key);
            }
            return result;
        }

        public Guid GetCurrentUserID()
        {
            return _inner.GetCurrentUserID();
        }
    }
}
