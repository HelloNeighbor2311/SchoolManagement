using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Course;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseController(ICourseService service) : ControllerBase
    {
        [HttpGet("Course")]
        [Authorize(Policy = PolicyConstants.CanViewCourses)]
        public async Task<ActionResult<List<CourseResponse>>> GetAllCourse()
        {
            var result = await service.GetAllCourse();
            return Ok(result);
        }
        [HttpGet("FilterCourse")]
        [Authorize(Policy = PolicyConstants.CanViewCourses)]
        public async Task<ActionResult<List<CourseResponse>>> GetCourseFilter([FromQuery] string name)
        {
            var result = await service.FilterCourseInformationByName(name);
            return Ok(result);
        }
        [HttpGet("id")]
        [Authorize(Policy = PolicyConstants.CanViewCourses)]
        public async Task<ActionResult<CourseResponse>> GetCourseById([FromQuery] int id)
        {
            var result = await service.GetCourseById(id);
            return Ok(result);
        }
        [HttpGet("detail")]
        public async Task<ActionResult<CourseDetailResponse>> GetCourseDetail([FromQuery] int id)
        {
            var result = await service.GetCourseDetail(id);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult<CourseResponse>> CreateCourse([FromBody]CreateCourseRequest request)
        {
            var result = await service.CreateCourse(request);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = PolicyConstants.AllMighty)]
        public async Task<ActionResult> DeleteCourse([FromQuery]int id)
        {
            await service.DeleteCourse(id);
            return NoContent();
        }
    }
}
