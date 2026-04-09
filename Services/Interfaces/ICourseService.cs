using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;

namespace SchoolManagement.Services.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseResponse>> GetAllCourse();
        Task<CourseResponse> GetCourseById(int courseId);
        Task<List<CourseResponse>> FilterCourseInformationByName(string name);
        Task<CourseResponse> CreateCourse(CreateCourseRequest request);
        Task DeleteCourse(int courseId);
        Task<CourseDetailResponse?> GetCourseDetail(int courseId);
    }
}
