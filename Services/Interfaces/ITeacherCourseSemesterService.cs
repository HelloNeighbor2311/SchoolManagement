using SchoolManagement.DTOs.TeacherCourseSemester;

namespace SchoolManagement.Services.Interfaces
{
    public interface ITeacherCourseSemesterService
    {
        Task<List<TeacherCourseSemesterResponse>?> GetAllTeacherCourseSemester();
        Task<TeacherCourseSemesterResponse> AllocateTeacherToCourse(AllocateTeacherCourseSemesterRequest request);
        Task DeleteTeacherFromCourse(int teacherCourseSemesterId);
    }
}
