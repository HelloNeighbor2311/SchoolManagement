using SchoolManagement.DTOs.CourseSemester;

namespace SchoolManagement.Services.Interfaces
{
    public interface ICourseSemesterService
    {
        Task<CourseSemesterResponse> CreateCourseSemester(CreateCourseSemesterRequest request);
        
    }
}
