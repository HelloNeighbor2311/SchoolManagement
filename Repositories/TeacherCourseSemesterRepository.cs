using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Linq.Expressions;


namespace SchoolManagement.Repositories
{
    public class TeacherCourseSemesterRepository(AppDbContext context) : GenericRepository<TeacherCourseSemester>(context),ITeacherCourseSemesterRepository
    {
        public async Task AllocateTeacherToCourseAsync(TeacherCourseSemester teacherCourseSemester)
        {
            await Context.TeacherCourseSemesters.AddAsync(teacherCourseSemester);
        }

        public async Task DeleteTeacherFromCourse(TeacherCourseSemester teacherCourseSemester)
        {
            Context.TeacherCourseSemesters.Remove(teacherCourseSemester);
        }

        public async Task<List<TeacherCourseSemester>?> GetAllTeacherCourseSemesterAsync()
        {
            var teacherCourseSemester =  await Context.TeacherCourseSemesters.Include(u => u.CourseSemester).ThenInclude(u => u!.Semester).Include(u => u.CourseSemester).ThenInclude(u => u!.Course).Include(u=>u.Teacher).ToListAsync();
            if (!teacherCourseSemester.Any()) return null;
            return teacherCourseSemester;
        }

        public async Task<TeacherCourseSemester?> GetTeacherCourseSemesterByIdAsync(int id)
        {
            return await Context.TeacherCourseSemesters.Include(u => u.CourseSemester).ThenInclude(u => u!.Semester).Include(u => u.CourseSemester).ThenInclude(u => u!.Course).Include(u => u.Teacher).FirstOrDefaultAsync(u => u.TeacherCourseSemesterId == id);
        }
    }
}
