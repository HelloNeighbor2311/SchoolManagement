using SchoolManagement.DTOs.Enrollment;

namespace SchoolManagement.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<List<EnrollmentResponse>> GetAllEnrollments();
        Task<EnrollmentResponse> RegisterEnrollment(int studentId, RegisterEnrollmentRequest request);
        Task DeleteEnrollment(int enrollmentId);
    }
}
