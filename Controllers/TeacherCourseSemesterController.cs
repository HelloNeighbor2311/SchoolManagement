using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherCourseSemesterController(ITeacherCourseSemesterService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<TeacherCouseSemesterResponse>>> GetAllTeacherCourseSemester()
        {
            var result = await service.GetAllTeacherCourseSemester();
            return Ok(result);
        }
    }
}
