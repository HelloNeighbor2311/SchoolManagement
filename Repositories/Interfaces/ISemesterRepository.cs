using SchoolManagement.DTOs.Semester;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ISemesterRepository 
    {
        Task<List<SemesterResponse>> GetAllSemesterAsync();
        Task<Semester?> GetSemesterByIdAsync(int semesterId);
        Task<Semester> CreateSemesterAsync(Semester semester);
        Task DeleteSemesterAsync(Semester semsester);
        Task <Semester?>GetSemesterDetailAsync(int semesterId);
    }
}
