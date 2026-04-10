using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController(IEnrollmentService service) : BaseApiController
    {
        [HttpGet]
        [Authorize(Policy = PermissionConstants.CanViewCourses)]
        public async Task<ActionResult<List<EnrollmentResponse>>> GetAllEnrollments()
        {
            var result = await service.GetAllEnrollments();
            return Ok(result);
        }
        [HttpPost("RegisterEnrollment")]
        [Authorize(Policy = PermissionConstants.ForStudent)]
        public async Task<ActionResult<EnrollmentResponse>> RegisterEnrollment([FromBody]RegisterEnrollmentRequest request)
        {
            if(!TryGetCurrentUserId(out int studentId))
            {
                return Unauthorized("Cannot find studentId");
            }
            var result = await service.RegisterEnrollment(studentId, request);
            return Ok(result);
        }
        [HttpDelete("DeleteEnrollment")]
        [Authorize(Policy = PermissionConstants.AllMighty)]
        public async Task<ActionResult> DeleteEnrollment([FromQuery]int id)
        {
            await service.DeleteEnrollment(id);
            return NoContent();
        }
    }
}
