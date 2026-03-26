using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Award;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardController(IAwardService service) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<AwardResponse>>> GetAllAwards()
        {
            var result = await service.GetAllAwards();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<AwardResponse>> CreateAward(CreateAwardRequest request)
        {
            var result = await service.CreateAward(request);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAward([FromQuery] int id)
        {
            await service.DeleteAward(id);
            return NoContent();
        }
    }
}
