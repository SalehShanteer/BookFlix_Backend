using BookFlix.Core.Service_Interfaces;
using BookFlix.Web.Dtos.User;
using BookFlix.Web.Mapper_Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace BookFlix.Web.Controllers
{
    [Authorize]
    [Route("api/Users")]
    public class UsersController : ApiController
    {
        public readonly IUserMapper _userMapper;
        public readonly IUserService _userService;

        public UsersController(IUserMapper userMapper, IUserService userService)
        {
            _userMapper = userMapper;
            _userService = userService;
        }

        [HttpGet("profile")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserProfileAsync()
        {
            var userId = _userService.GetCurrentUserID();
            var result = await _userService.GetUserByIDAsync(userId);
            if (result.IsFailure) return HandleFailure(result);
            UserDto userDto = _userMapper.ToUserDto(result.Value);
            return Ok(userDto);
        }

        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUserPasswordAsync(UserUpdatePasswordDto userUpdatePasswordDto)
        {
            var userId = _userService.GetCurrentUserID();
            var result = await _userService.UpdateUserPasswordAsync(userId, userUpdatePasswordDto.OldPassword, userUpdatePasswordDto.NewPassword);
            if (result.IsFailure) return HandleFailure(result);
            return NoContent();
        }

        [HttpPut("username")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserUsernameAsync(UserUpdateUsernameDto userUpdateUsernameDto)
        {
            var userId = _userService.GetCurrentUserID();
            var result = await _userService.UpdateUserUsernameAsync(userId, userUpdateUsernameDto.Username);
            if (result.IsFailure) return HandleFailure(result);
            var userDto = _userMapper.ToUserDto(result.Value);
            return Ok(userDto);
        }

        [HttpPut("email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserEmailAsync(UserUpdateEmailDto userUpdateEmailDto)
        {
            var userId = _userService.GetCurrentUserID();
            var result = await _userService.UpdateUserEmailAsync(userId, userUpdateEmailDto.Email);
            if (result.IsFailure) return HandleFailure(result);
            var userDto = _userMapper.ToUserDto(result.Value);
            return Ok(userDto);
        }

        [HttpGet("ProfileImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserProfileImageAsync()
        {
            var userId = _userService.GetCurrentUserID();
            var fileResult = await _userService.GetUserProfilePathAsync(userId);
            if (fileResult.IsFailure) return HandleFailure(fileResult);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileResult.Value, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(fileResult.Value, contentType);
        }

        [HttpPut("ProfileImage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
        {
            var userId = _userService.GetCurrentUserID();
            var result = await _userService.UploadProfileImageAsync(userId, file);

            if (result.IsFailure) return HandleFailure(result);

            return Ok(new { FileID = result.Value });
        }
    }
}
