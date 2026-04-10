using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Models;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AwardApprovalController(IAwardApprovalService service) : BaseApiController
    {
        [HttpPost]
        [Authorize(Policy = PolicyConstants.ForAdminOnly)]
        public async Task<ActionResult<AwardApprovalResponse>> CreateAwardApproval([FromBody]CreateAwardApprovalRequest request)
        {
            var result = await service.CreateAwardApproval(request);
            return Ok(result);
        }
        [HttpGet]
        [Authorize(Policy = PolicyConstants.ForAdminOnly)]
        public async Task<ActionResult<List<AwardApprovalResponse>>> GetAllAwardApprovals()
        {
            var result = await service.GetAllAwardApprovals();
            return Ok(result);
        }
        [HttpGet("ListByTeacherId")]
        [Authorize(Policy = PolicyConstants.TeacherAndAdmin)]
        public async Task<ActionResult<List<AwardApprovalResponse>>> GetListAwardApprovalsViaTeacherId([FromQuery] int teacherId)
        {
            var result = await service.GetAwardApprovalsViaTeacherId(teacherId);
            return Ok(result);
        }
        [HttpPut]
        [Authorize(Policy = PolicyConstants.ForAdminOnly)]
        public async Task<ActionResult> UpdateAwardApproval([FromQuery]int id,[FromBody] UpdateAwardApprovalRequest request)
        {
            await service.UpdateAwardApproval(id, request);
            return NoContent();
        }
        [HttpPut("Teacher")]
        public async Task<ActionResult> UpdateAwardApprovalViaTeacherId([FromQuery]int id, [FromBody] UpdateAwardApprovalRequest request)
        {
            if (IsTeacher())
            {
                var userID = GetCurrentUserId();
                await service.UpdateAwardApprovalForTeacher(id, userID, request);
                return NoContent();
            }
            return Forbid("You don't have permission");
            
        }
        [HttpDelete]
        [Authorize(Policy = PolicyConstants.ForAdminOnly)]
        public async Task<ActionResult> DeleteAwardApproval([FromQuery]int id)
        {
            await service.DeleteAwardApproval(id);
            return NoContent();
        }
    }
}
