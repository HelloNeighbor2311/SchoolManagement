using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AwardApprovalController(IAwardApprovalService service) : BaseApiController
    {
        [HttpPost]
        public async Task<ActionResult<AwardApprovalResponse>> CreateAwardApproval([FromBody]CreateAwardApprovalRequest request)
        {
            var result = await service.CreateAwardApproval(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<ActionResult<List<AwardApprovalResponse>>> GetAllAwardApprovals()
        {
            var result = await service.GetAllAwardApprovals();
            return Ok(result);
        }
        [HttpGet("ListByTeacherId")]
        public async Task<ActionResult<List<AwardApprovalResponse>>> GetListAwardApprovalsViaTeacherId([FromQuery] int teacherId)
        {
            var result = await service.GetAwardApprovalsViaTeacherId(teacherId);
            return Ok(result);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateAwardApproval([FromQuery]int id,[FromBody] UpdateAwardApprovalRequest request)
        {
            await service.UpdateAwardApproval(id, request);
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteAwardApproval([FromQuery]int id)
        {
            await service.DeleteAwardApproval(id);
            return NoContent();
        }
    }
}
