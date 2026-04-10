using SchoolManagement.DTOs.CourseSemester;

namespace SchoolManagement.Services.Interfaces
{
    public interface ICourseSemesterService
    {
        Task<CourseSemesterResponse> CreateCourseSemester(CreateCourseSemesterRequest request);
        Task<List<CourseSemesterResponse>> GetAllCourseSemesters();
        Task DeleteCourseSemester(int courseSemesterId);
        
    }
}
