using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseRepository{
        Task<List<Models.Course>> GetAllCourseAsync();
        Task<List<Models.Course>?> FilterCourseInformationByNameAsync(string name);
        Task<Models.Course?> CreateCourseAsync(Course request);
        Task<Models.Course?> GetCourseByIdAsync(int id);
        Task DeleteCourseAsync(Models.Course course);
        Task<Course?> GetCourseDetailAsync(int id);
    }
}
