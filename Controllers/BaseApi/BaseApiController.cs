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
        protected bool TryGetCurrentUserId(out int userId)
        {
            userId = 0;
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)
                     ?? User.FindFirst("sub");
            return claim is not null && int.TryParse(claim.Value, out userId);
        }

        protected int GetCurrentUserId()
        {
            if (!TryGetCurrentUserId(out int userId))
                throw new UnauthorizedException("Không tìm thấy thông tin người dùng.");
            return userId;
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
