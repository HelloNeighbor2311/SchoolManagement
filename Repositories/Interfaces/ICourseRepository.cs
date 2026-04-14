using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseRepository{
        Task<List<CourseResponse>> GetAllCourseAsync();
        Task<List<CourseResponse>?> FilterCourseInformationByNameAsync(string name);
        Task<Models.Course?> CreateCourseAsync(Course request);
        Task<Models.Course?> GetCourseByIdAsync(int courseId);
        Task DeleteCourseAsync(Models.Course course);
        Task<CourseDetailResponse?> GetCourseDetailAsync(int courseId);
        Task<CourseResponse?> GetCourseResponseByIdAsync(int courseId);
    }
}
