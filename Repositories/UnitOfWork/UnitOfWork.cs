using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext context;
        public IUserRepository Users { get; }
        public ICourseRepository Courses { get; }
        public ISemesterRepository Semesters { get; }
        public ITeacherCourseSemesterRepository TeacherCourseSemester { get; }
        public UnitOfWork(AppDbContext context, IUserRepository userRepository, 
            ICourseRepository courseRepository, ISemesterRepository semesterRepository, ITeacherCourseSemesterRepository teacherCourseSemesterRepository)
        {
            this.context = context;
            Users = userRepository;
            Courses = courseRepository;
            Semesters = semesterRepository;
            TeacherCourseSemester = teacherCourseSemesterRepository;
        }
        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose() => context.Dispose();
    }
}
