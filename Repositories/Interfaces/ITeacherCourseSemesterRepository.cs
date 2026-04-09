using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface ITeacherCourseSemesterRepository: IGenericRepository<TeacherCourseSemester>
    {
        Task<List<TeacherCourseSemester>?> GetAllTeacherCourseSemesterAsync();
        Task AllocateTeacherToCourseAsync(TeacherCourseSemester teacherCourseSemester);
        Task DeleteTeacherFromCourse(TeacherCourseSemester teacherCourseSemester);
        Task<TeacherCourseSemester?> GetTeacherCourseSemesterByIdAsync(int teacherCourseSemesterId);
    }
}
