using SchoolManagement.DTOs;
using SchoolManagement.Models;

namespace SchoolManagement.Services
{
    public interface IUserService
    {
        Task<object> CreateUser(CreateUserResponse request);
        Task<List<UserResponse>> GetAllUsers();
        Task<UserResponse?> GetUserByUsername(string username);
        Task UpdateUser(int id, UpdateUserResponse request);
        Task DeleteUser(int id);
        Task<PageResult<UserResponse>> GetPageResultUsers(PaginationParam param);
    }
}
