using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using SchoolManagement.Datas;
using SchoolManagement.DTOs;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System;

namespace SchoolManagement.Repositories
{
    public class UserRepository(AppDbContext context): GenericRepository<User>(context), IUserRepository
    {
        public async Task<User?> CreateUserAsync(User user)
        {
            if (await Context.Users.AnyAsync(p => p.Username == user.Username)) return null;
            await Context.Users.AddAsync(user);
            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            Context.Users.Remove(user);
        }

        public async Task<List<User>> GetAllUserAsync() => await Context.Users.Include(u => u.Role).ToListAsync();

        public async Task<IEnumerable<User>> GetPageResultAsync(int pageSize, int pageNum)
        {
            var count = await GetTotalUser();
            var query = Context.Users.Include(u => u.Role).AsQueryable();
            var sortedUsers = await query.OrderBy(u => u.UserId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToListAsync();
            return sortedUsers;
        }

        public async Task<int> GetTotalUser() =>  await Context.Users.CountAsync();

        public async Task<User?> GetUserByIdAsync(int id) => await Context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> GetUserByUsernameAsync(string username) => await Context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<User?> GetWithRoleAsync(string username)
            => await Context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == username);

        public async Task<bool> IsTeacherAsync(int id)
        {
            var teacher = await GetUserByIdAsync(id);
            if (teacher is null) return false;
            return teacher.RoleId == 3;
        }
        public async Task<bool> IsStudentAsync(int id)
        {
            var student = await GetUserByIdAsync(id);
            if (student is null) return false;
            return student.RoleId == 2;
        }

        public async Task UpdateUserAsync(User user)
        {
            Context.Users.Update(user);
        }
    }
}
