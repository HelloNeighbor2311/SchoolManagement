using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherCourseSemesterController(ITeacherCourseSemesterService service) : BaseApiController
    {
        [HttpGet]
        [Authorize(Policy = PolicyConstants.TeacherAndAdmin)]
        public async Task<ActionResult<List<TeacherCourseSemesterResponse>>> GetAllTeacherCourseSemester()
        {
            var result = await service.GetAllTeacherCourseSemester();
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult<TeacherCourseSemesterResponse>> AllocateTeacherToCourse([FromBody] AllocateTeacherCourseSemesterRequest request)
        {
            var result = await service.AllocateTeacherToCourse(request);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult> DeleteTeacherFromCourse([FromQuery] int id)
        {
            await service.DeleteTeacherFromCourse(id);
            return NoContent();
        }
    }
}
