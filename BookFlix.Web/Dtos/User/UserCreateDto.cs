using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserCreateDto
    {
        [MinLength(4, ErrorMessage = "UsernameLengthTooShort")]
        public string Username { get; set; }

        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}