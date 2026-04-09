using SchoolManagement.DTOs;
using SchoolManagement.DTOs.User;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User?> CreateUserAsync(User request);

        Task<List<User>> GetAllUserAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int userId);
        Task<UserResponse?> GetUserResponseByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<IEnumerable<User>> GetPageResultAsync(int pageSize, int pageNum);
        Task<int> GetTotalUser();
        Task<bool> IsTeacherAsync(int userId);
        Task<bool> IsStudentAsync(int userId);

        void SetRowVersion(User user, byte[] rowVersion);
    }
}
