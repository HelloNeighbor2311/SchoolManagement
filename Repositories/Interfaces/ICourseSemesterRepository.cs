using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseSemesterRepository: IGenericRepository<CourseSemester>
    {
        Task<CourseSemester?> GetCourseSemesterByIdAsync(int courseSemesterId);
        Task CreateCourseSemesterAsync(CourseSemester courseSemester);
        Task DeletetCourseSemesterAsync(CourseSemester courseSemester);
    }
}
