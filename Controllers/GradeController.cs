using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController(IGradeService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GradeResponse>> GetAllGradeWithStudentId([FromQuery]int id)
        {
            var result = await service.GetGradeWithStudentId(id);
            return Ok(result);
        }
    }
}
