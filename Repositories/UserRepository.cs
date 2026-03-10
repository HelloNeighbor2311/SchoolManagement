using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SchoolManagement.Datas;
using SchoolManagement.DTOs;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories
{
    public class UserRepository: GenericRepository<User>,IUserRepository
    {
        public UserRepository(AppDbContext context): base(context){}
        public async Task<User?> CreateUserAsync(User user)
        {
            if (await context.Users.AnyAsync(p => p.Username == user.Username)) return null;
            await context.AddAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            context.Users.Remove(user);
        }

        public async Task<List<User>> GetAllUserAsync() => await context.Users.Include(u => u.Role).ToListAsync();

        public async Task<User?> GetUserByIdAsync(int id) => await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> GetUserByUsernameAsync(string username) => await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> GetWithRoleAsync(int userId)
           => await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == userId);

        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);
        }
    }
}
