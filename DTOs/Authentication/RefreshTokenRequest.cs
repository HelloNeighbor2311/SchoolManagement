using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.DTOs.Authentication
{
    public class RefreshTokenRequest
    {
        [Required]
        public required string RefreshToken { get; set; }

    }
}
