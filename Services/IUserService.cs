using SchoolManagement.DTOs;

namespace SchoolManagement.Services
{
    public interface IUserService
    {
        Task<object> CreateUser(CreateUserResponse request);
        Task<List<UserResponse>> GetAllUsers();
    }
}
