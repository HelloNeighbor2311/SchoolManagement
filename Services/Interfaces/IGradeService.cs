using SchoolManagement.DTOs.Grade;

namespace SchoolManagement.Services.Interfaces
{
    public interface IGradeService
    {
        Task<List<GradeResponse>> GetGradeWithStudentId(int studentId);
        Task UpdateGrade(int gradeId, UpdateGradeRequest request);
    }
}
