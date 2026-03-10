using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using SchoolManagement.DTOs;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.UnitOfWork;

namespace SchoolManagement.Services
{
    public class UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork uow) : IUserService
    {
        public async Task<object> CreateUser(CreateUserResponse request)
        {
            if (request.RoleId == 2 && request.EnrollYear == null) throw new BadRequestException("Student is missing EnrollYear");
            if (request.RoleId == 3 && request.Speciality == null) throw new BadRequestException("Teacher is missing Speciality");
            User user = request.RoleId switch
            {
                1 => mapper.Map<Admin>(request),
                2 => mapper.Map<Student>(request),
                3 => mapper.Map<Teacher>(request),
                _ => throw new BadRequestException($"Invalid Role Id: {request.RoleId}")
            };
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.PasswordHashed = hashedPassword;

            await userRepository.CreateUserAsync(user);
            await uow.SaveChangeAsync();

            var savedUser = await userRepository.GetWithRoleAsync(user.UserId);
            return mapper.Map<UserResponse>(savedUser);
        }

        public async Task DeleteUser(int id)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if (user is null) throw new BadRequestException($"User with the given id {id} was not found !");
            await userRepository.DeleteUserAsync(user);
            await uow.SaveChangeAsync();
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var user = await userRepository.GetAllUserAsync();
            if(user is null) { throw new NotFoundException("Users was not found"); }
            var userResponse = user.Select(u => mapper.Map<UserResponse>(u)).ToList();
            return userResponse;
        }

        public async Task<UserResponse?> GetUserByUsername(string username)
        {
            var user = await userRepository.GetUserByUsernameAsync(username);
            if(user is null) { throw new NotFoundException("User with the given username was not found!"); }
            return mapper.Map<UserResponse>(user);
        }

        public async Task UpdateUser(int id,UpdateUserResponse request)
        {
            var user = await userRepository.GetUserByIdAsync(id);
            if(user is null) { throw new NotFoundException($"User with the given id {id} was not found !!"); }
            if (!string.IsNullOrEmpty(request.Password)) {
                var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
                user.PasswordHashed = hashedPassword;
            }
            user.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : user.Name;
            user.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : user.Email;

            switch (user)
            {
                case Admin admin:
                    await userRepository.UpdateUserAsync(admin);
                    break;
                case Student student:
                    if (request.EnrollYear.HasValue) student.EnrollYear = request.EnrollYear.Value;
                    await userRepository.UpdateUserAsync(student);
                    break;
                case Teacher teacher:
                    if (!string.IsNullOrEmpty(request.Speciality)) teacher.Speciality = request.Speciality;
                    await userRepository.UpdateUserAsync(teacher);
                    break;
            }
            await uow.SaveChangeAsync();
        }
    }
}
