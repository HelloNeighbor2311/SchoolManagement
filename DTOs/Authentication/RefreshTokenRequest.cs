using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Authentication
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;

    }
}
