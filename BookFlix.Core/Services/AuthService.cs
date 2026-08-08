using BookFlix.Core.Helpers;
using BookFlix.Core.Models;
using BookFlix.Core.Repositories;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Core.Services.Validation;
using static BookFlix.Core.Enums.GeneralEnums;

namespace BookFlix.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserLogRepository _userLogRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IUserLogRepository userLogRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _userLogRepository = userLogRepository;
            _jwtService = jwtService;
        }

        public async Task<Result<(string AccessToken, string RefreshToken)>> LoginAsync(string email, string password, string ipAddress)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<(string, string)>(Error.Validation("EmailIsEmpty"));
            }

            var user = await _userRepository.GetByEmailAsync(email);

            if (user is null)
            {
                return Result.Failure<(string, string)>(Error.NotFound("UserNotFound"));
            }

            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                await LogLoginAttempt(user.ID, false, ipAddress);

                return Result.Failure<(string, string)>(Error.Validation("InvalidPassword"));
            }

            await LogLoginAttempt(user.ID, true, ipAddress);
            return await ReturnTokens(user);
        }

        private async Task LogLoginAttempt(Guid userID, bool isSuccess, string ipAddress)
        {
            var log = new UserLog
            {
                UserID = userID,
                EventType = EventType.Login,
                Success = isSuccess,
                IpAddress = ipAddress,
            };
            await _userLogRepository.AddAsync(log);
        }

        private async Task<Result<(string AccessToken, string RefreshToken)>> ReturnTokens(User user)
        {
            string accessToken = _jwtService.GenerateJwtToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken(user.ID);
            user.RefreshTokens.Add(refreshToken);
            await _userRepository.SaveChangesAsync();

            return Result.Success((accessToken, refreshToken.Token));
        }
    }
}
