using SchoolManagement.DTOs.Enrollment;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IEnrollmentRepository: IGenericRepository<Enrollment>
    {
        Task<List<Enrollment>> GetAllEnrollmentInformationAsync();
        Task RegisterEnrollmentAsync(Enrollment request);
        Task<Enrollment?> GetEnrollmentByIdAsync(int id);
        Task DeleteEnrollmentAsync(Enrollment enrollment);
    }
}
