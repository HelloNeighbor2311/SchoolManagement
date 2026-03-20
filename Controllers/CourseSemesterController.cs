using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Services;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseSemesterController (ICourseSemesterService service): ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CourseSemesterResponse>> CreateCourseSemester([FromBody] CreateCourseSemesterRequest request)
        {
            var result = await service.CreateCourseSemester(request);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult<CourseSemesterResponse>> DeleteCourseSemester([FromQuery] int id)
        {
            await service.DeleteCourseSemester(id);
            return NoContent();
        }
    }
}
