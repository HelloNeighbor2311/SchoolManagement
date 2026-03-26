using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeController(IGradeService service) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<GradeResponse>> GetAllGradeWithStudentId([FromQuery]int id)
        {
            var result = await service.GetGradeWithStudentId(id);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateGrade([FromQuery]int id, [FromBody]UpdateGradeRequest request)
        {
            await service.UpdateGrade(id, request);
            return NoContent();
        }
    }
}
