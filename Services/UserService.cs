using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using SchoolManagement.DTOs.User;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.UnitOfWork;

namespace SchoolManagement.Services
{
    public class UserService( IMapper mapper, IUnitOfWork uow) : IUserService
    {
        public async Task<UserResponse> CreateUser(CreateUserResponse request)
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

            var checkedUser = await uow.Users.CreateUserAsync(user);
            if (checkedUser is null) throw new BadRequestException("The current username has already existed");
            await uow.SaveChangeAsync();

            var savedUser = await uow.Users.GetUserByIdAsync(user.UserId);
            return mapper.Map<UserResponse>(savedUser);
        }

        public async Task DeleteUser(int id)
        {
            var user = await uow.Users.GetUserByIdAsync(id);
            if (user is null) throw new BadRequestException($"User with the given id {id} was not found !");
            await uow.Users.DeleteUserAsync(user);
            await uow.SaveChangeAsync();
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            var user = await uow.Users.GetAllUserAsync();
            if(user is null) { throw new NotFoundException("Users was not found"); }
            var userResponse = user.Select(u => mapper.Map<UserResponse>(u)).ToList();
            return userResponse;
        }

        public async Task<PageResult<UserResponse>> GetPageResultUsers(PaginationParam param)
        {
            var listUser = await uow.Users.GetPageResultAsync(param.PageSize, param.PageNumber) ?? new List<User>();
            var userCount = await uow.Users.GetTotalUser();
            var listUserResponse = listUser.Select(u => mapper.Map<UserResponse>(u)).ToList();
            return new PageResult<UserResponse>(listUserResponse, userCount, param.PageNumber, param.PageSize);
        }

        public async Task<UserResponse?> GetUserByUsername(string username)
        {
            var user = await uow.Users.GetUserByUsernameAsync(username);
            if(user is null) { throw new NotFoundException("User with the given username was not found!"); }
            return mapper.Map<UserResponse>(user);
        }
        public async Task<UserResponse?> GetUserById(int id)
        {
            var user = await uow.Users.GetUserByIdAsync(id);
            if(user is null) { throw new NotFoundException("User with the given id was not found!"); }
            return mapper.Map<UserResponse>(user);
        }

        public async Task UpdateUser(int id,UpdateUserRequest request)
        {
            var user = await uow.Users.GetUserByIdAsync(id);
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
                    await uow.Users.UpdateUserAsync(admin);
                    break;
                case Student student:
                    if (request.EnrollYear.HasValue) student.EnrollYear = request.EnrollYear.Value;
                    await uow.Users.UpdateUserAsync(student);
                    break;
                case Teacher teacher:
                    if (!string.IsNullOrEmpty(request.Speciality)) teacher.Speciality = request.Speciality;
                    await uow.Users.UpdateUserAsync(teacher);
                    break;
            }
            await uow.SaveChangeAsync();
        }
    }
}
