using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GradeController(IGradeService service) : BaseApiController
    {
        [HttpGet]
        [Authorize(Policy = PermissionConstants.ForStudent)]
        public async Task<ActionResult<GradeResponse>> GetAllGradeWithStudentId()
        {
            if (!TryGetCurrentUserId(out int id)) return Unauthorized("Cannot find userId");
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
