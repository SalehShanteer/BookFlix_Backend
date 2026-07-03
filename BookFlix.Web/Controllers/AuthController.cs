using Azure.Core;
using BookFlix.Core.Helpers;
using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.Auth;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookFlix.Web.Controllers
{
    [Route("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IUserMapper _userMapper;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthService authService, IUserService userService, IUserMapper userMapper, IJwtService jwtService, IConfiguration configuration)
        {
            _authService = authService;
            _userService = userService;
            _userMapper = userMapper;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignupAsync(UserCreateDto userCreateDto)
        {
            var user = _userMapper.ToUser(userCreateDto);
            var result = await _userService.AddUserAsUserAsync(user);

            if (result.IsFailure) return HandleFailure(result);
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var authResult = await _authService.LoginAsync(userCreateDto.Email, userCreateDto.Password, ipAddress);
            if (authResult.IsFailure) return HandleFailure(authResult);

            Response.SetTokenCookies(authResult.Value.AccessToken, authResult.Value.RefreshToken, _configuration);

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("signup/admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> SignupAsAdminAsync(UserCreateDto userCreateDto)
        {
            var user = _userMapper.ToUser(userCreateDto);
            var result = await _userService.AddUserAsAdminAsync(user);

            if (result.IsFailure) return HandleFailure(result);

            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var authResult = await _authService.LoginAsync(userCreateDto.Email, userCreateDto.Password, ipAddress);

            if (authResult.IsFailure) return HandleFailure(authResult);

            Response.SetTokenCookies(authResult.Value.AccessToken, authResult.Value.RefreshToken, _configuration);

            return Ok();
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto loginDto)
        {
            string ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password, ipAddress);

            if (result.IsFailure) return HandleFailure(result);

            Response.SetTokenCookies(result.Value.AccessToken, result.Value.RefreshToken, _configuration);

            return Ok();
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenDto refreshToken)
        {
            var result = await _userService.UpdateUserRefreshTokenAsync(refreshToken.Token);

            if (result.IsFailure) return HandleFailure(result);

            Response.SetTokenCookies(result.Value.AccessToken, result.Value.RefreshToken, _configuration);

            return Ok();
        }

        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> LogoutAsync()
        {
            var refreshToken = Request.GetRefreshTokenFromCookies();
            await _userService.RevokeUserRefreshTokenAsync(refreshToken);

            return Ok();
        }

        [HttpPost("is-authenticated")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> IsAuthenticatedAsync([FromBody] RefreshTokenDto refreshToken)
        {
            var isAuthenticated = await _jwtService.IsValidRefreshToken(refreshToken.Token);

            return Ok(isAuthenticated);
        }
    }
}