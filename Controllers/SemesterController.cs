using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Semester;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Models;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SemesterController(ISemesterService service) : BaseApiController
    {
        [HttpPost]
        [Authorize(Policy = PermissionConstants.AllMighty)]
        public async Task<ActionResult<SemesterResponse>> CreateSemester(CreateSemesterRequest request)
        {
            var result = await service.CreateSemester(request);
            return Ok(result);
        }
        [HttpGet]
        [Authorize(Policy = PermissionConstants.TeacherAndAdmin)]
        public async Task<ActionResult<List<SemesterResponse>>> GetAllSemester()
        {
            var result = await service.GetAllSemester();
            return Ok(result);
        }
        [HttpGet("id")]
        [Authorize(Policy = PermissionConstants.TeacherAndAdmin)]
        public async Task<ActionResult<SemesterResponse>> GetSemesterById([FromQuery] int id)
        {
            var result = await service.GetSemesterById(id);
            return Ok(result);
        }
        [HttpGet("detail")]
        [Authorize(Policy = PermissionConstants.TeacherAndAdmin)]
        public async Task<ActionResult<SemesterDetailResponse>> GetSemesterDetail([FromQuery] int id)
        {
            var result = await service.GetSemesterDetail(id);
            return Ok(result);
        }
        [HttpDelete]
        [Authorize(Policy = PermissionConstants.AllMighty)]
        public async Task<ActionResult> DeleteSemester([FromQuery] int id)
        {
            await service.DeleteSemester(id);
            return NoContent();
        }
    }
}
