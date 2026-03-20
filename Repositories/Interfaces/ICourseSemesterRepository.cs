using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ICourseSemesterRepository: IGenericRepository<CourseSemester>
    {
        Task<CourseSemester?> GetCourseSemesterById(int id);
        Task CreateCourseSemester(CourseSemester courseSemester);
    }
}
