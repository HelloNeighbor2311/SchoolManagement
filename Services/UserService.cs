using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SchoolManagement.DTOs.User;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class UserService( IMapper mapper, IUnitOfWork uow, ILogger<UserService> logger) : IUserService
    {
        public async Task<UserResponse> CreateUser(CreateUserResponse request)
        {
            using (logger.BeginOperationScope("CreateUser", ("RoleId", request.RoleId)))
            using (var timer = logger.TimeOperation("CreateUser"))
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
                try
                {
                    var checkedUser = await uow.User.CreateUserAsync(user);
                    if (checkedUser is null) throw new BadRequestException("The current username has already existed");
                    await uow.SaveChangeAsync();
                    var savedUser = await uow.User.GetUserByIdAsync(user.UserId);
                    return mapper.Map<UserResponse>(savedUser);
                }catch(Exception e)
                {
                    logger.LogOperationError("CreateUser", e, request.RoleId);
                    throw;
                }
            }
        }

        public async Task DeleteUser(int id)
        {
            using (logger.BeginOperationScope("DeleteUser", ("UserId", id)))
            using (var timer = logger.TimeOperation("GetAllTeacherCourseSemester"))
            {
                var user = await uow.User.GetUserByIdAsync(id);
                if (user is null) throw new BadRequestException($"User with the given id {id} was not found !");
                try
                {
                    await uow.User.DeleteUserAsync(user);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted<User>("User", id);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteUser", e, id);
                    throw;
                }
            }
        }

        public async Task<List<UserResponse>> GetAllUsers()
        {
            using (logger.BeginOperationScope("GetAllUsers"))
            using (var timer = logger.TimeOperation("GetAllUsers"))
            {
                var user = await uow.User.GetAllUserAsync();
                if (user is null) { throw new NotFoundException("Users was not found"); }
                var userResponse = user.Select(u => mapper.Map<UserResponse>(u)).ToList();
                return userResponse;
            }
        }

        public async Task<PageResult<UserResponse>> GetPageResultUsers(PaginationParam param)
        {
            using (logger.BeginOperationScope("GetPageResultUsers"))
            using (var timer = logger.TimeOperation("GetPageResultUsers"))
            {
                var listUser = await uow.User.GetPageResultAsync(param.PageSize, param.PageNumber) ?? new List<User>();
                var userCount = await uow.User.GetTotalUser();
                var listUserResponse = listUser.Select(u => mapper.Map<UserResponse>(u)).ToList();
                return new PageResult<UserResponse>(listUserResponse, userCount, param.PageNumber, param.PageSize);
            }
        }

        public async Task<UserResponse?> GetUserByUsername(string username)
        {
            using (logger.BeginOperationScope("GetUserByName", ("Username", username)))
            using (var timer = logger.TimeOperation("GetUserByName"))
            {
                var user = await uow.User.GetUserByUsernameAsync(username);
                if (user is null) { throw new NotFoundException("User with the given username was not found!"); }
                return mapper.Map<UserResponse>(user);
            }
        }
        public async Task<UserResponse?> GetUserById(int id)
        {
            using (logger.BeginOperationScope("GetUserById", ("UserId", id)))
            using (var timer = logger.TimeOperation("GetUserById"))
            {
                var user = await uow.User.GetUserByIdAsync(id);
                if (user is null) { throw new NotFoundException("User with the given id was not found!"); }
                return mapper.Map<UserResponse>(user);
            }
        }

        public async Task<UserResponse> UpdateUser(int id,UpdateUserRequest request)
        {
            using (logger.BeginOperationScope("UpdateUser", ("UserId", id)))
            using (var timer = logger.TimeOperation("UpdateUser"))
            {
                var user = await uow.User.GetUserByIdAsync(id);
                if (user is null) { throw new NotFoundException($"User with the given id {id} was not found !!"); }
                if (!string.IsNullOrEmpty(request.Password))
                {
                    var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
                    user.PasswordHashed = hashedPassword;
                }
                var rowVersionBytes = Convert.FromBase64String(request.RowVersion);
                uow.User.SetRowVersion(user, rowVersionBytes);
                user.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : user.Name;
                user.Email = !string.IsNullOrEmpty(request.Email) ? request.Email : user.Email;

                switch (user)
                {
                    case Admin admin:
                        if (!string.IsNullOrEmpty(request.Speciality)) throw new BadRequestException("Admin doesn't have field Speciality");
                        if (request.EnrollYear.HasValue) throw new BadRequestException("Admin doesn't have field EnrollYear");
                        await uow.User.UpdateUserAsync(admin);
                        break;
                    case Student student:
                        if (request.EnrollYear.HasValue) student.EnrollYear = request.EnrollYear.Value;
                        if (!string.IsNullOrEmpty(request.Speciality)) throw new BadRequestException("Student doesn't have field Speciality");
                        await uow.User.UpdateUserAsync(student);
                        break;
                    case Teacher teacher:
                        if (request.EnrollYear.HasValue) throw new BadRequestException("Teacher doesn't have field EnrollYear");
                        if (!string.IsNullOrEmpty(request.Speciality)) teacher.Speciality = request.Speciality;
                        await uow.User.UpdateUserAsync(teacher);
                        break;
                }
                try
                {
                    await uow.SaveChangeAsync();
                    return await uow.User.GetUserResponseByIdAsync(id);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new ConflictException("User has been modified by someone. Please try again later");
                }
            }
        }
    }
}
