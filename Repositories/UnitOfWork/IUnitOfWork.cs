using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        ICourseRepository Courses { get; }
        ISemesterRepository Semesters { get; }
        ITeacherCourseSemesterRepository TeacherCourseSemester { get; }
        
        Task<int> SaveChangeAsync();
    }
}
