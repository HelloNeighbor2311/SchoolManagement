using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.Services.Interfaces;
using System.Security.Claims;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController (IAuthService service): BaseApiController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var response = await service.LoginAsync(request);
            return Ok(response);
        }
        [HttpPost("register {student}")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentRequest request)
        {
            var response = await service.RegisterStudentAsync(request);
            return Ok(response);
        }
        [HttpPost("{register teacher}")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherRequest request)
        {
            var response = await service.RegisterTeacherAsync(request);
            return Ok(response);
        }
        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var response = await service.RefreshTokenAsync(request);
            return Ok(response);
        }
        [HttpPost("revoke")]
        [AllowAnonymous]
        public async Task<IActionResult> Revoke([FromBody] string refreshToken)
        {
            await service.RevokeTokenAsync(refreshToken);
            return NoContent();
        }
        [HttpPost("revoke-all")]
        [Authorize]
        public async Task<IActionResult> RevokeAll()
        {
            var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier) ?? User.FindFirst("sub");

            if (userClaimId is null || !int.TryParse(userClaimId.Value, out int userId)) return Unauthorized("Invalid token claims");
            await service.RevokeAllTokenAsync(userId);
            return NoContent();
        }
    }
}
