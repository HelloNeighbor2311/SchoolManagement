using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs;
using SchoolManagement.Models;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService service): ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(CreateUserResponse request)
        {
            var user = await service.CreateUser(request);
            if (user is null) return BadRequest("Username is already existed !");

            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            var user = await service.GetAllUsers();
            if (user is null) return BadRequest("An error occured");

            return Ok(user);
        }
    }
}
