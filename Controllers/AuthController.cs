using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Authentication;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController (IAuthService service): ControllerBase
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
        [HttpPost("{teacher}")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterTeacher([FromBody] RegisterTeacherRequest request)
        {
            var response = await service.RegisterTeacherAsync(request);
            return Ok(response);
        }
    }
}
