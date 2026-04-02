namespace SchoolManagement.DTOs.Authentication
{
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
