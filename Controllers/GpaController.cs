using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Gpa;
using SchoolManagement.Middleware.Authorizations;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GpaController(IGpaService service) : BaseApiController
    {
        [HttpGet]
        [Authorize(Policy = PolicyConstants.TeacherAndAdmin)]
        public async Task<ActionResult<List<GpaResponse>>> GetAllGpas()
        {
            var result = await service.GetAllGpas();
            return Ok(result);
        }
    }
}
