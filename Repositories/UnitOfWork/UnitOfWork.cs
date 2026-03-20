using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext context;
        public IAuthRepository Auth { get; set; }
        public IUserRepository User { get; }
        public ICourseRepository Course { get; }
        public ISemesterRepository Semester { get; }
        public ITeacherCourseSemesterRepository TeacherCourseSemester { get; }
        public ICourseSemesterRepository CourseSemester { get; }
        public UnitOfWork(AppDbContext context, IAuthRepository authRepository,IUserRepository userRepository, 
            ICourseRepository courseRepository, ISemesterRepository semesterRepository, ITeacherCourseSemesterRepository teacherCourseSemesterRepository,
            ICourseSemesterRepository courseSemester)
        {
            this.context = context;
            Auth = authRepository;
            User = userRepository;
            Course = courseRepository;
            Semester = semesterRepository;
            CourseSemester = courseSemester;
            TeacherCourseSemester = teacherCourseSemesterRepository;
        }
        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose() => context.Dispose();
    }
}
