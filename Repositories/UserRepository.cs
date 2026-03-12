using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SchoolManagement.Datas;
using SchoolManagement.DTOs;
using SchoolManagement.Models;
using System;

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

        public async Task<IEnumerable<User>> GetPageResultAsync(int pageSize, int pageNum)
        {
            var count = await GetTotalUser();
            var query = context.Users.Include(u => u.Role).AsQueryable();
            var sortedUsers = await query.OrderBy(u => u.UserId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return sortedUsers;
        }

        public async Task<int> GetTotalUser() =>  await context.Users.CountAsync();

        public async Task<User?> GetUserByIdAsync(int id) => await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> GetUserByUsernameAsync(string username) => await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> GetWithRoleAsync(string username)
            => await context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task UpdateUserAsync(User user)
        {
            context.Users.Update(user);
        }
    }
}
