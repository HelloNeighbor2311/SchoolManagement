using SchoolManagement.DTOs.Semester;
using SchoolManagement.Models;

namespace SchoolManagement.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<SemesterResponse> CreateSemester(CreateSemesterRequest request);
        Task<List<SemesterResponse>> GetAllSemester();
        Task<SemesterResponse> GetSemesterById(int id);
        Task<SemesterDetailResponse> GetSemesterDetail(int id);
        Task DeleteSemester(int id);
    }
}
