using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Exceptions;
using System.Security.Claims;

namespace SchoolManagement.Controllers.BaseApi
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim is null) throw new UnauthorizedException("User Id not found in token");
            return int.Parse(userIdClaim);
        }
        protected bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }
        protected bool IsTeacher()
        {
            return User.IsInRole("Teacher");
        }
        protected bool IsStudent()
        {
            return User.IsInRole("Student");
        }
    }
}
