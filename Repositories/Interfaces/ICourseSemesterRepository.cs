using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseSemesterRepository: IGenericRepository<CourseSemester>
    {
        Task<List<CourseSemesterResponse>> GetAllCourseSemesters();
        Task<CourseSemester?> GetCourseSemesterByIdAsync(int courseSemesterId);
        Task<CourseSemesterResponse?> GetCourseSemesterResponseByIdAsync(int courseSemesterId);
        Task CreateCourseSemesterAsync(CourseSemester courseSemester);
        Task DeletetCourseSemesterAsync(CourseSemester courseSemester);
    }
}
