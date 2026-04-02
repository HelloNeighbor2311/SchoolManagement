using SchoolManagement.DTOs;
using SchoolManagement.DTOs.User;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> CreateUserAsync(User request);
        Task<User?> GetWithRoleAsync(string username);

        Task<List<User>> GetAllUserAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<UserResponse?> GetUserResponseByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<IEnumerable<User>> GetPageResultAsync(int pageSize, int pageNum);
        Task<int> GetTotalUser();
        Task<bool> IsTeacherAsync(int id);
        Task<bool> IsStudentAsync(int id);

        void SetRowVersion(User user, byte[] rowVersion);
    }
}
