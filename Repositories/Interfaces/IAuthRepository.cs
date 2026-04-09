using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string token);
        Task<List<RefreshToken>?> GetActivedTokensByUserIdAsync(int userId);
        Task RevokeTokenAsync(RefreshToken token);
        Task RevokeAllTokenByUserIdAsync(int userId);
    }
}
