using SchoolManagement.DTOs.Authentication;

namespace SchoolManagement.Services.Interfaces
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(UserLoginRequest request);
        Task<TokenResponse> RegisterStudentAsync(RegisterStudentRequest request);
        Task<TokenResponse> RegisterTeacherAsync(RegisterTeacherRequest request);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
        Task RevokeTokenAsync(string refreshToken);
        Task RevokeAllTokenAsync(int userId);
    }
}
