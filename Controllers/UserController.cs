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
        public async Task<ActionResult<User>> CreateUser(CreateUserResponse request)
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
        [HttpGet("username")]
        public async Task<ActionResult<List<UserResponse>>> GetUserById(string username)
        {
            var user = await service.GetUserByUsername(username);
            if (user is null) return NotFound("The user with the entered username is not existed");

            return Ok(user);
        }
        [HttpGet("pagination")]
        public async Task<ActionResult<List<UserResponse>>> GetUserByPage([FromQuery]PaginationParam param)
        {
            var user = await service.GetPageResultUsers(param);
            if (user is null) return BadRequest("There was an unexpected error occured. Please try again later");

            return Ok(user);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UpdateUserResponse request)
        {
            await service.UpdateUser(id, request);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteUser([FromQuery] int id)
        {
            await service.DeleteUser(id);
            return NoContent();
        }
    }
}
