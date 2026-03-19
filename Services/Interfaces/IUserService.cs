using SchoolManagement.DTOs.User;
using SchoolManagement.Models;

namespace SchoolManagement.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUser(CreateUserResponse request);
        Task<List<UserResponse>> GetAllUsers();
        Task<UserResponse?> GetUserByUsername(string username);
        Task<UserResponse?> GetUserById(int id);
        Task UpdateUser(int id, UpdateUserRequest request);
        Task DeleteUser(int id);
        Task<PageResult<UserResponse>> GetPageResultUsers(PaginationParam param);
    }
}
