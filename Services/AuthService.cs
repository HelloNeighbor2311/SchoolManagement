using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolManagement.DTOs;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories;
using SchoolManagement.Repositories.UnitOfWork;

namespace SchoolManagement.Services
{
    public class AuthService(IUnitOfWork uow, IAuthRepository authRepository, IJWTService jwtService, IMapper mapper) : IAuthService
    {
        public async Task<TokenResponse> LoginAsync(UserLoginRequest request)
        {
            var user = await uow.Users.GetWithRoleAsync(request.Username) ?? throw new UnauthorizedException("Invalid username or password");
            var verifiedPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHashed, request.Password);
            if (verifiedPassword == PasswordVerificationResult.Failed) throw new UnauthorizedException("Invalid username or password");
            return await BuildAuthResponseAsync(user);
        }

        public async Task<TokenResponse> RegisterStudentAsync(RegisterStudentRequest request)
        {
            var student = mapper.Map<Student>(request);
            if (student is null) throw new BadRequestException("An unexpected error occured ! Please try again later");
            var hashedPassword = new PasswordHasher<User>().HashPassword(student, request.Password);
            student.PasswordHashed = hashedPassword;
            var savedStudent = await uow.Users.CreateUserAsync(student);
            if (savedStudent is null) throw new UnauthorizedException("The current username is already existed!");
            await uow.SaveChangeAsync();

            var newStudent = await uow.Users.GetWithRoleAsync(savedStudent.UserId) ?? throw new NotFoundException("User was not found after registered");
            return await BuildAuthResponseAsync(newStudent);

        }

        public async Task<TokenResponse> RegisterTeacherAsync(RegisterTeacherRequest request)
        {
            var teacher = mapper.Map<Teacher>(request);
            if (teacher is null) throw new BadRequestException("An unexpected error occured ! Please try again later");
            var hashedPassword = new PasswordHasher<User>().HashPassword(teacher, request.Password);
            teacher.PasswordHashed = hashedPassword;
            var savedTeacher = await uow.Users.CreateUserAsync(teacher);
            if (savedTeacher is null) throw new BadRequestException("The current username is already existed!");
            await uow.SaveChangeAsync();

            var newTeacher = await uow.Users.GetWithRoleAsync(savedTeacher.UserId) ?? throw new NotFoundException("User was not found after registered");
            return await BuildAuthResponseAsync(newTeacher);
        }

        private async Task<TokenResponse> BuildAuthResponseAsync(User user)
        {
            var accessToken = jwtService.GenerateAccessToken(user);
            var refreshTokenStr = jwtService.GenerateRefreshToken();
            var accessExpiry = jwtService.GetAccessTokenExpiry();
            var refreshExpiry = jwtService.GetRefreshTokenExpiry();

            var refreshToken = new RefreshToken
            {
                UserId = user.UserId,
                Token = refreshTokenStr,
                ExpiredDate = refreshExpiry,
                IsRevoked = false
            };

            await authRepository.AddRefreshTokenAsync(refreshToken);
            await uow.SaveChangeAsync();

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenStr,
                User = mapper.Map<UserResponse>(user)
            };
        }
    }
}
