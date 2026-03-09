using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using SchoolManagement.DTOs;
using SchoolManagement.Models;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.UnitOfWork;

namespace SchoolManagement.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork uow) : IUserService
    {
        public async Task<object> CreateUser(CreateUserResponse request)
        {
            if (request.RoleId == 2 && request.EnrollYear == null) throw new Exception("Student is missing EnrollYear");
            if (request.RoleId == 3 && request.Speciality == null) throw new Exception("Teacher is missing Speciality");
            User user = request.RoleId switch
            {
                1 => mapper.Map<Admin>(request),
                2 => mapper.Map<Student>(request),
                3 => mapper.Map<Teacher>(request),
                _ => throw new BadHttpRequestException("Invalid RoleId")
            };
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.PasswordHashed = hashedPassword;

            await userRepository.CreateUserAsync(user);
            await uow.SaveChangeAsync();

            var savedUser = await userRepository.GetWithRoleAsync(user.UserId);
            return mapper.Map<UserResponse>(savedUser);
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var user = await userRepository.GetAllUserAsync();
            var userResponse = user.Select(u => mapper.Map<UserResponse>(u)).ToList();
            return userResponse;
        }
    }
}
