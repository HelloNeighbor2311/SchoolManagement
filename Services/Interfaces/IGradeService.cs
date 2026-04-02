using SchoolManagement.DTOs.Grade;

namespace SchoolManagement.Services.Interfaces
{
    public interface IGradeService
    {
        Task<List<GradeResponse>> GetGradeWithStudentId(int id);
        Task UpdateGrade(int id, UpdateGradeRequest request);
    }
}
