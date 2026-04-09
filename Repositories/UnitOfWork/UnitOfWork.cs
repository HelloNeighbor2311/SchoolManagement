using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext context;
        public IAuthRepository Auth { get; }
        public IUserRepository User { get; }
        public ICourseRepository Course { get; }
        public ISemesterRepository Semester { get; }
        public ITeacherCourseSemesterRepository TeacherCourseSemester { get; }
        public ICourseSemesterRepository CourseSemester { get; }
        public IEnrollmentRepository Enrollment { get; }
        public IGradeRepository Grade { get; }
        public IGpaRepository Gpa { get; }
        public IAwardRepository Award { get; }
        public IAwardApprovalRepository AwardApproval { get; }
        public UnitOfWork(AppDbContext context, IAuthRepository authRepository,IUserRepository userRepository, 
            ICourseRepository courseRepository, ISemesterRepository semesterRepository, ITeacherCourseSemesterRepository teacherCourseSemesterRepository,
            ICourseSemesterRepository courseSemester, IEnrollmentRepository enrollmentRepository, IGradeRepository gradeRepository,
            IGpaRepository gpaRepository, IAwardRepository awardRepository, IAwardApprovalRepository awardApprovalRepository)
        {
            this.context = context;
            Auth = authRepository;
            User = userRepository;
            Course = courseRepository;
            Semester = semesterRepository;
            CourseSemester = courseSemester;
            TeacherCourseSemester = teacherCourseSemesterRepository;
            Enrollment = enrollmentRepository;
            Grade = gradeRepository;
            Gpa = gpaRepository;
            Award = awardRepository;
            AwardApproval = awardApprovalRepository;
        }
        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose() => context.Dispose();
    }
}
