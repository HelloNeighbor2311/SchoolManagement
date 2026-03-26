using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Controllers.BaseApi;
using SchoolManagement.DTOs.Gpa;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GpaController(IGpaService service) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<GpaResponse>>> GetAllGpas()
        {
            var result = await service.GetAllGpas();
            return Ok(result);
        }
    }
}
