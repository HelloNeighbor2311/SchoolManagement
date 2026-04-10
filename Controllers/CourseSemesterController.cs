using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseSemesterController (ICourseSemesterService service): BaseApiController
    {
        [HttpGet]
        [Authorize(Policy = PolicyConstants.AuthenticatedUsers)]
        public async Task<ActionResult<List<CourseSemesterResponse>>> GetAllCourseSemesters()
        {
            var result = await service.GetAllCourseSemesters();
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = PolicyConstants.ForAdminOnly)]
        public async Task<ActionResult<CourseSemesterResponse>> CreateCourseSemester([FromBody] CreateCourseSemesterRequest request)
        {
            var result = await service.CreateCourseSemester(request);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = PolicyConstants.ForAdminOnly)]
        public async Task<ActionResult<CourseSemesterResponse>> DeleteCourseSemester([FromQuery] int id)
        {
            await service.DeleteCourseSemester(id);
            return NoContent();
        }
    }
}
