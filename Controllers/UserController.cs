using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.User;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Models;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService service): ControllerBase
    {
        [HttpPost("CreateUser")]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult<User>> CreateUser(CreateUserResponse request)
        {
            var user = await service.CreateUser(request);
            if (user is null) return BadRequest("Username is already existed !");

            return Ok(user);
        }
        [HttpGet]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            var user = await service.GetAllUsers();
            if (user is null) return BadRequest("An error occured");

            return Ok(user);
        }
        [HttpGet("username")]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult<UserResponse>> GetUserByUsername(string username)
        {
            var user = await service.GetUserByUsername(username);
            if (user is null) return NotFound("The user with the entered username is not existed");

            return Ok(user);
        }
        [HttpGet("{id:int}")]
        [Authorize(Policy = PolicyConstants.CanViewUserDetail)]
        public async Task<ActionResult<UserResponse>> GetUserById([FromRoute]int id)
        {
            var user = await service.GetUserById(id);
            if (user is null) return NotFound("The user with the entered username is not existed");

            return Ok(user);
        }
        [HttpGet("pagination")]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult<List<UserResponse>>> GetUserByPage([FromQuery]PaginationParam param)
        {
            var user = await service.GetPageResultUsers(param);
            if (user is null) return BadRequest("There was an unexpected error occured. Please try again later");

            return Ok(user);
        }
        [HttpPut("{id:int}")]
        [Authorize(Policy = PolicyConstants.CanViewUserDetail)]
        public async Task<ActionResult> UpdateUser([FromRoute]int id, [FromBody] UpdateUserRequest request)
        {
            await service.UpdateUser(id, request);
            return NoContent();
        }
        [HttpDelete]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult> DeleteUser([FromQuery] int id)
        {
            await service.DeleteUser(id);
            return NoContent();
        }
    }
}
