using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.DTOs.User;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;
using System.Security.Claims;

namespace SchoolManagement.Services
{
    public class AuthService(IUnitOfWork uow, IJWTService jwtService, IMapper mapper) : IAuthService
    {
        public async Task<TokenResponse> LoginAsync(UserLoginRequest request)
        {
            var user = await uow.User.GetWithRoleAsync(request.Username) ?? throw new UnauthorizedException("Invalid username or password");
            var verifiedPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHashed, request.Password);
            if (verifiedPassword == PasswordVerificationResult.Failed) throw new UnauthorizedException("Invalid username or password");
            return await BuildAuthResponseAsync(user);
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            //validate access token
            var principal = jwtService.GetClaimsPrincipalFromExpiredToken(request.AccessToken) ?? throw new UnauthorizedException("Invalid Access Token");
            //find userId via claim
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
            if(userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedException("Invalid claim tokens");
            }
            var storedToken = await uow.Auth.GetRefreshTokenAsync(request.RefreshToken) ?? throw new UnauthorizedException("Invalid refresh token");
            if (storedToken.UserId != userId) throw new UnauthorizedException("Token is mismatched");
            if (storedToken.IsRevoked) throw new UnauthorizedException("Token is revoked");
            if (storedToken.ExpiredDate < DateTime.UtcNow) throw new UnauthorizedException("Token is expired");
            await uow.Auth.RevokeTokenAsync(storedToken);

            var storedTokenUser = storedToken.User;
            if (storedTokenUser is null) throw new BadRequestException("This token doesn't include user");
            return await BuildAuthResponseAsync(storedTokenUser);
        }

        public async Task<TokenResponse> RegisterStudentAsync(RegisterStudentRequest request)
        {
            var student = mapper.Map<Student>(request);
            if (student is null) throw new BadRequestException("An unexpected error occured ! Please try again later");
            var hashedPassword = new PasswordHasher<User>().HashPassword(student, request.Password);
            student.PasswordHashed = hashedPassword;
            var savedStudent = await uow.User.CreateUserAsync(student);
            if (savedStudent is null) throw new UnauthorizedException("The current username is already existed!");
            await uow.SaveChangeAsync();

            var newStudent = await uow.User.GetUserByIdAsync(savedStudent.UserId) ?? throw new NotFoundException("User was not found after registered");
            return await BuildAuthResponseAsync(newStudent);

        }

        public async Task<TokenResponse> RegisterTeacherAsync(RegisterTeacherRequest request)
        {
            var teacher = mapper.Map<Teacher>(request);
            if (teacher is null) throw new BadRequestException("An unexpected error occured ! Please try again later");
            var hashedPassword = new PasswordHasher<User>().HashPassword(teacher, request.Password);
            teacher.PasswordHashed = hashedPassword;
            var savedTeacher = await uow.User.CreateUserAsync(teacher);
            if (savedTeacher is null) throw new BadRequestException("The current username is already existed!");
            await uow.SaveChangeAsync();

            var newTeacher = await uow.User.GetUserByIdAsync(savedTeacher.UserId) ?? throw new NotFoundException("User was not found after registered");
            return await BuildAuthResponseAsync(newTeacher);
        }

        public async Task RevokeAllTokenAsync(int userId)
        {
            var user = uow.User.GetUserByIdAsync(userId);
            if (user is null) throw new BadRequestException($"The user with the given user ID {userId} is not existed");
            await uow.Auth.RevokeAllTokenByUserIdAsync(userId);
            await uow.SaveChangeAsync();
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var storedToken = await uow.Auth.GetRefreshTokenAsync(refreshToken) ?? throw new UnauthorizedException("Invalid refresh token");
            if (storedToken.IsRevoked) throw new UnauthorizedException("This token is already revoked");
            await uow.Auth.RevokeTokenAsync(storedToken);
            await uow.SaveChangeAsync();
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

            await uow.Auth.AddRefreshTokenAsync(refreshToken);
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
