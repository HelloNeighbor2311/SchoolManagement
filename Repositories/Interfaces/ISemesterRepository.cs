using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ISemesterRepository 
    {
        Task<List<Semester>> GetAllSemesterAsync();
        Task<Semester?> GetSemesterByIdAsync(int semesterId);
        Task<Semester> CreateSemesterAsync(Semester semester);
        Task DeleteSemesterAsync(Semester semsester);
        Task <Semester?>GetSemesterDetailAsync(int semesterId);
    }
}
