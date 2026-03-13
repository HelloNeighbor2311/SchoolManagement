using SchoolManagement.DTOs.User;

namespace SchoolManagement.DTOs.Authentication
{
    public class TokenResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }

        public UserResponse User { get; set; } = null!;
    }
}
