using SchoolManagement.DTOs.Enrollment;

namespace SchoolManagement.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentResponse>> GetAllEnrollments();
        Task<EnrollmentResponse> RegisterEnrollment(RegisterEnrollmentRequest request);
        Task DeleteEnrollment(int id);
    }
}
