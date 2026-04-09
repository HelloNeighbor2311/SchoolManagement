using SchoolManagement.DTOs.Grade;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IGradeRepository
    {
        Task<List<GradeResponse>> GetAllGradeWithStudentIdAsync(int studentId);
        Task<bool> isAllGradedAsync(int studentId, int semesterId);
        Task UpdateGradeAsync(Grade grade);
        Task<Grade?> GetGradeByIdAsync(int gradeId);
        void SetRowVersion(Grade grade, byte[] rowVersion);
    }
}
