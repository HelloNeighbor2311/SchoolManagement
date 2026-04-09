using SchoolManagement.DTOs.Semester;
using SchoolManagement.Models;

namespace SchoolManagement.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<SemesterResponse> CreateSemester(CreateSemesterRequest request);
        Task<List<SemesterResponse>> GetAllSemester();
        Task<SemesterResponse> GetSemesterById(int semesterId);
        Task<SemesterDetailResponse> GetSemesterDetail(int semesterId);
        Task DeleteSemester(int semesterId);
    }
}
