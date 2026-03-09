using SchoolManagement.DTOs;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> CreateUserAsync(User request);
        public Task<User?> GetWithRoleAsync(int userId);
        Task<List<User>> GetAllUserAsync();
    }
}
