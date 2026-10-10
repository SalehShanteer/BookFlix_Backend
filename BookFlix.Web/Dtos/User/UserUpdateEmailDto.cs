using System.ComponentModel.DataAnnotations;

namespace BookFlix.Web.Dtos.User
{
    public class UserUpdateEmailDto
    {
        [Required(ErrorMessage = "EmailRequired")]
        [EmailAddress(ErrorMessage = "InvalidEmail")]
        public string Email { get; set; }
    }

}