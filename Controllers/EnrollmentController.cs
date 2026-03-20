using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController(IEnrollmentService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<EnrollmentResponse>>> GetAllEnrollments()
        {
            var result = await service.GetAllEnrollments();
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponse>> RegisterEnrollment([FromBody]RegisterEnrollmentRequest request)
        {
            var result = await service.RegisterEnrollment(request);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteEnrollment([FromQuery]int id)
        {
            await service.DeleteEnrollment(id);
            return NoContent();
        }
    }
}
