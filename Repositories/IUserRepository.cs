using SchoolManagement.DTOs;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> CreateUserAsync(User request);
        Task<User?> GetWithRoleAsync(int userId);
        Task<List<User>> GetAllUserAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
    }
}
