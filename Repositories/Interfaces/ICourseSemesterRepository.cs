using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseSemesterRepository: IGenericRepository<CourseSemester>
    {
        Task<CourseSemester?> GetCourseSemesterByIdAsync(int id);
        Task CreateCourseSemesterAsync(CourseSemester courseSemester);
        Task DeletetCourseSemesterAsync(CourseSemester courseSemester);
    }
}
