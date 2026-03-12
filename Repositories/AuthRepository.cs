using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories
{
    public class AuthRepository(AppDbContext context) : IAuthRepository
    {
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await context.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task<List<RefreshToken>?> GetActivedTokensByUserIdAsync(int id)
        {
            var refreshTokens = await context.RefreshTokens.Where(u => u.UserId == id && u.ExpiredDate > DateTime.UtcNow && !u.IsRevoked).ToListAsync();
            return refreshTokens;
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
        {
            return await context.RefreshTokens.Include(u => u.User).ThenInclude(u => u.Role).FirstOrDefaultAsync(rt => rt.Token == token);
        }

        public async Task RevokeAllTokenByUserIdAsync(int id)
        {
            var activedRefreshToken = await GetActivedTokensByUserIdAsync(id);
            if (activedRefreshToken is null) return;
            foreach(var i in activedRefreshToken)
            {
                i.IsRevoked = true;
            }
            context.RefreshTokens.UpdateRange(activedRefreshToken);
        }

        public async Task RevokeTokenAsync(RefreshToken token)
        {
            token.IsRevoked = true;
            context.RefreshTokens.Update(token);
        }
    }
}
