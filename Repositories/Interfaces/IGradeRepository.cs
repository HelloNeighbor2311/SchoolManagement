using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<List<Grade>> GetAllGradeWithStudentIdAsync(int id);
    }
}
