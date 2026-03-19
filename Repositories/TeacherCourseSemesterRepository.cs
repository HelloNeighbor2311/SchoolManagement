using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;


namespace SchoolManagement.Repositories
{
    public class TeacherCourseSemesterRepository(AppDbContext context) : ITeacherCourseSemesterRepository
    {
        public async Task AllocateTeacherToCourseAsync(TeacherCourseSemester teacherCourseSemester)
        {
            await context.TeacherCourseSemester.AddAsync(teacherCourseSemester);
        }

        public async Task<List<TeacherCourseSemester>?> GetAllTeacherCourseSemesterAsync()
        {
            var teacherCourseSemester =  await context.TeacherCourseSemester.Include(u => u.CourseSemester).ThenInclude(u => u!.Semester).Include(u => u.CourseSemester).ThenInclude(u => u!.Course).Include(u=>u.Teacher).ToListAsync();
            if (!teacherCourseSemester.Any()) return null;
            return teacherCourseSemester;
        }
        
    }
}
