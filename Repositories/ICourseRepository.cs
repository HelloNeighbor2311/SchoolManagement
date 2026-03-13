using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories
{
    public interface ICourseRepository{
        Task<List<Models.Course>> GetAllCourseAsync();
        Task<List<Models.Course>?> FilterCourseInformationByNameAsync(string name);
        Task<Models.Course?> CreateCourseAsync(Models.Course request);
        Task<Models.Course?> GetCourseByIdAsync(int id);
        Task DeleteCourseAsync(Models.Course course);
    }
}
