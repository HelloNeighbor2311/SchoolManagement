using SchoolManagement.DTOs.Gpa;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IGpaRepository: IGenericRepository<Gpa>
    {
        Task<List<GpaResponse>> GetAllGpaAsync();
        Task AddGpaAsync(Gpa gpa);
        Task<Gpa?> FindGpaViaStudentIdAndSemesterIdAsync(int studentId, int semesterId);
        Task<Gpa?> FindGpaViaIdAsync(int gpaId);

        Task<(double? gpa, int totalCredit)> CaculateGpaAsync(int studentId, int semesterId);
        Task UpdateGpaAsync(Gpa gpa);
    }
}
