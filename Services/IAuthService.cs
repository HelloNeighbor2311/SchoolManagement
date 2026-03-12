using SchoolManagement.DTOs.Authentication;

namespace SchoolManagement.Services
{
    public interface IAuthService
    {
        Task<TokenResponse> LoginAsync(UserLoginRequest request);
        Task<TokenResponse> RegisterStudentAsync(RegisterStudentRequest request);
        Task<TokenResponse> RegisterTeacherAsync(RegisterTeacherRequest request);
    }
}
