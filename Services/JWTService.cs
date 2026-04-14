using Microsoft.IdentityModel.Tokens;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.Models;
using SchoolManagement.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SchoolManagement.Services
{
    public class JWTService(IConfiguration configuration) : IJWTService
    {
        private readonly string _secretKey = configuration["JwtSettings:SecretKey"]!;
        private readonly string _issuer = configuration["JwtSettings:Issuer"]!;
        private readonly string _audience = configuration["JwtSettings:Audience"]!;
        private readonly int accessTokenExpiry = int.Parse(configuration["JwtSettings:AccessTokenExpiryMinutes"]!);
        private readonly int refreshTokenExpiry = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"]!);
        public string GenerateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var roleName = user.Role?.RoleName?.Trim();
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new SecurityTokenException("User role is missing. Cannot generate access token for authorization.");
            }

            var listClaims = new List<Claim> {
                new (JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new (ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new (JwtRegisteredClaimNames.Name, user.Name),
                new (JwtRegisteredClaimNames.Email, user.Email),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.Role, roleName),
                new ("role", roleName)
            };
            var token = new JwtSecurityToken(
                issuer: this._issuer,
                audience: this._audience,
                claims: listClaims,
                expires: GetAccessTokenExpiry(),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public DateTime GetAccessTokenExpiry()
        {
            return DateTime.UtcNow.AddMinutes(accessTokenExpiry);
        }

        public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey))
            };
            var handler = new JwtSecurityTokenHandler();
            var principal = handler.ValidateToken(token, tokenValidationParams, out var validatedToken);

            if(validatedToken is not JwtSecurityToken jwtToken || !jwtToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token Algorithm");
            }
            return principal;
        }

        public DateTime GetRefreshTokenExpiry()
        {
            return DateTime.UtcNow.AddDays(refreshTokenExpiry);
        }
    }
}
