using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IAuthRepository Auth { get; set; }
        IUserRepository User { get; }
        ICourseRepository Course { get; }
        ISemesterRepository Semester { get; }
        ITeacherCourseSemesterRepository TeacherCourseSemester { get; }
        ICourseSemesterRepository CourseSemester { get; }
        IGradeRepository Grade { get; }
        IEnrollmentRepository Enrollment { get; }
        
        Task<int> SaveChangeAsync();
    }
}
