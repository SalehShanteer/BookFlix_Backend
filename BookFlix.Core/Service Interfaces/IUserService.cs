using BookFlix.Core.Models;
using BookFlix.Core.Services.Validation;
using Microsoft.AspNetCore.Http;

namespace BookFlix.Core.Service_Interfaces
{
    public interface IUserService
    {
        Task<Result<User>> AddUserAsUserAsync(User user);
        Task<Result<User>> AddUserAsAdminAsync(User user);
        Task<Result<User>> GetUserByIDAsync(Guid id);
        Task<User> GetUserByRefreshTokenAsync(string token);
        Task<Result> UpdateUserPasswordAsync(Guid userID, string oldPassword, string newPassword);
        Task<Result<User>> UpdateUserUsernameAsync(Guid id, string username);
        Task<Result<User>> UpdateUserEmailAsync(Guid id, string email);
        Task<Result<(string AccessToken, string RefreshToken)>> UpdateUserRefreshTokenAsync(string refreshToken);
        Task RevokeUserRefreshTokenAsync(string refreshToken);
        Task<IReadOnlyCollection<User>> GetAllUsersAsync();
        Task<Result<string>> GetUserProfilePathAsync(Guid userID);
        Task<Result<Guid>> UploadProfileImageAsync(Guid userID, IFormFile file);
        Guid GetCurrentUserID();
    }
}
