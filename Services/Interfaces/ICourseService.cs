using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;

namespace SchoolManagement.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseResponse>> GetAllCourse();
        Task<CourseResponse> GetCourseById(int id);
        Task<List<CourseResponse>> FilterCourseInformationByName(string name);
        Task<CourseResponse> CreateCourse(CreateCourseRequest request);
        Task DeleteCourse(int id);
        Task<CourseDetailResponse?> GetCourseDetail(int id);
    }
}
