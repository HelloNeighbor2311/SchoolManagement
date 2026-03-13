using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Course;
using SchoolManagement.Services;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController(ICourseService service) : ControllerBase
    {
        [HttpGet("Course")]
        public async Task<ActionResult<List<CourseResponse>>> GetAllCourse()
        {
            var result = await service.GetAllCourse();
            return Ok(result);
        }
        [HttpGet("FilterCourse")]
        public async Task<ActionResult<List<CourseResponse>>> GetCourseFilter([FromQuery] string name)
        {
            var result = await service.FilterCourseInformationByName(name);
            return Ok(result);
        }
        [HttpGet("id")]
        public async Task<ActionResult<CourseResponse>> GetCourseById([FromQuery] int id)
        {
            var result = await service.GetCourseById(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<CourseResponse>> CreateCourse([FromBody]CreateCourseRequest request)
        {
            var result = await service.CreateCourse(request);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteCourse([FromQuery]int id)
        {
            await service.DeleteCourse(id);
            return NoContent();
        }
    }
}
