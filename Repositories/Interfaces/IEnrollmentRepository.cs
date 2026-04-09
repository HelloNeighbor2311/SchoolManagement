using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IEnrollmentRepository: IGenericRepository<Enrollment>
    {
        Task<List<EnrollmentResponse>> GetAllEnrollmentInformationAsync();
        Task RegisterEnrollmentAsync(Enrollment request);
        Task<Enrollment?> GetEnrollmentByIdAsync(int enrollmentId);
        Task DeleteEnrollmentAsync(Enrollment enrollment);
    }
}
