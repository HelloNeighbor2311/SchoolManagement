using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Award;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AwardController(IAwardService service) : BaseApiController
    {
        [HttpGet]
        [Authorize(Policy = PermissionConstants.TeacherAndAdmin)]
        public async Task<ActionResult<List<AwardResponse>>> GetAllAwards()
        {
            var result = await service.GetAllAwards();
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = PermissionConstants.AllMighty)]
        public async Task<ActionResult<AwardResponse>> CreateAward(CreateAwardRequest request)
        {
            var result = await service.CreateAward(request);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = PermissionConstants.AllMighty)]
        public async Task<ActionResult> DeleteAward([FromQuery] int id)
        {
            await service.DeleteAward(id);
            return NoContent();
        }
        [HttpPut]
        [Authorize(Policy = PermissionConstants.AllMighty)]
        public async Task<ActionResult<AwardResponse>> UpdateAward([FromQuery] int id, [FromBody] UpdateAwardRequest request)
        {
            var result = await service.UpdateAward(id, request);
            return Ok(result);
        }
    }
}
