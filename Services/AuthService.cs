using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.DTOs.User;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;
using System.Security.Claims;

namespace SchoolManagement.Services
{
    public class AuthService(IUnitOfWork uow, IJWTService jwtService, IMapper mapper, ILogger<AuthService> logger) : IAuthService
    {
        public async Task<TokenResponse> LoginAsync(UserLoginRequest request)
        {
            using (logger.BeginOperationScope("Login", ("Username", request.Username)))
            using (var timer = logger.TimeOperation("LoginAsync"))
            {
                logger.LogOperationStart("LoginAsync", request.Username);

                var user = await uow.User.GetUserByUsernameAsync(request.Username);

                if (user == null)
                {
                    logger.LogLoginFailed(request.Username);
                    throw new UnauthorizedException("Invalid username or password");
                }

                var verifiedPassword = new PasswordHasher<User>()
                    .VerifyHashedPassword(user, user.PasswordHashed, request.Password);

                if (verifiedPassword == PasswordVerificationResult.Failed)
                {
                    throw new UnauthorizedException("Invalid username or password");
                }

                var response = await BuildAuthResponseAsync(user);

                logger.LogUserLogin(user.UserId, user.Username);

                return response;
            }
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            using (logger.BeginOperationScope("RefreshToken"))
            using (var timer = logger.TimeOperation("RefreshTokenAsync"))
            {
                logger.LogOperationStart("RefreshToken");
                //validate access token
                var principal = jwtService.GetClaimsPrincipalFromExpiredToken(request.AccessToken) ?? throw new UnauthorizedException("Invalid Access Token");
                //find userId via claim
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier) ?? principal.FindFirst("sub");
                if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    logger.LogValidationWarning("Access token", "Invalid claim");
                    throw new UnauthorizedException("Invalid claim tokens");
                }
                var storedToken = await uow.Auth.GetRefreshTokenAsync(request.RefreshToken) ?? throw new UnauthorizedException("Invalid refresh token");
                if (storedToken.UserId != userId)
                {
                    logger.LogWarning("Token mismatch - Expected UserId: {ExpectedUserId}, Got: {ActualUserId}", userId, storedToken.UserId);
                    throw new UnauthorizedException("Token is mismatched");
                }
                if (storedToken.IsRevoked) {
                    throw new UnauthorizedException("Token is revoked"); 
                }
                if (storedToken.ExpiredDate < DateTime.UtcNow) { 
                    logger.LogWarning("Expired refresh token used for UserId: {UserId}", userId); 
                    throw new UnauthorizedException("Token is expired"); 
                }
                await uow.Auth.RevokeTokenAsync(storedToken);

                var storedTokenUser = storedToken.User;
                if (storedTokenUser is null) throw new BadRequestException("This token doesn't include user");
                logger.LogTokenRefreshed(userId);
                return await BuildAuthResponseAsync(storedTokenUser);
            }
            
        }

        public async Task<TokenResponse> RegisterStudentAsync(RegisterStudentRequest request)
        {
            using (logger.BeginOperationScope("RegisterStudent", ("Username", request.Username)))
            using (var timer = logger.TimeOperation("RegisterStudentAsync"))
            {
                var student = mapper.Map<Student>(request);
                if (student is null) throw new BadRequestException("An unexpected error occured when trying to map student! Please try again later");
                var hashedPassword = new PasswordHasher<User>().HashPassword(student, request.Password);
                student.PasswordHashed = hashedPassword;
                var savedStudent = await uow.User.CreateUserAsync(student);
                if (savedStudent is null) {
                    logger.LogValidationWarning("Username", "Already existed!");
                    throw new UnauthorizedException("The current username is already existed!"); }
                await uow.SaveChangeAsync();
                logger.LogUserRegistered(savedStudent.UserId, request.Username, "Student");
                var newStudent = await uow.User.GetUserByIdAsync(savedStudent.UserId) ?? throw new NotFoundException("User was not found after registered");
                return await BuildAuthResponseAsync(newStudent);
            }

        }
        public async Task RevokeAllTokenAsync(int userId)
        {
            using (logger.BeginOperationScope("RevokeAllToken", ("UserId", userId)))
            using (var timer = logger.TimeOperation("RevokeAllTokenAsync"))
            {
                var user = await uow.User.GetUserByIdAsync(userId);
                if (user is null) {
                    logger.LogEntityNotFound("User", userId);
                    throw new BadRequestException($"The user with the given user ID {userId} is not existed"); 
                }
                await uow.Auth.RevokeAllTokenByUserIdAsync(userId);
                await uow.SaveChangeAsync();
                logger.LogTokenRevoked(userId);
            }
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            using (logger.BeginOperationScope("RevokeToken", ("RefreshToken", refreshToken)))
            using (var timer = logger.TimeOperation("RevokeTokenAsync"))
            {
                var storedToken = await uow.Auth.GetRefreshTokenAsync(refreshToken) ?? throw new UnauthorizedException("Invalid refresh token");
                if (storedToken.IsRevoked) throw new UnauthorizedException("This token is already revoked");
                await uow.Auth.RevokeTokenAsync(storedToken);
                await uow.SaveChangeAsync();
                logger.LogTokenRevoked(storedToken.UserId);
            }
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
