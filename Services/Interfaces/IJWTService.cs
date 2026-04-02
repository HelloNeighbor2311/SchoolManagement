using SchoolManagement.DTOs.Authentication;
using SchoolManagement.Models;
using System.Security.Claims;

namespace SchoolManagement.Services.Interfaces
{
    public interface IJWTService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();

        ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token);
        DateTime GetAccessTokenExpiry();
        DateTime GetRefreshTokenExpiry();
    }
}
