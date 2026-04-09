using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Authentication
{
    public class UserLoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
