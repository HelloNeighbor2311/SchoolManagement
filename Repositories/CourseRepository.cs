using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Collections;

namespace SchoolManagement.Repositories
{
    public class CourseRepository(AppDbContext context) : ICourseRepository
    {
        public async Task<Models.Course?> CreateCourseAsync(Course request)
        {
            if (await context.Courses.AnyAsync(p => p.CourseName == request.CourseName)) return null;
            await context.Courses.AddAsync(request);
            return request;
        }

        public async Task DeleteCourseAsync(Course course)
        {
            context.Courses.Remove(course);
        }

        public async Task<List<Course>> GetAllCourseAsync()
        {
            var courses = await context.Courses.ToListAsync();
            return courses;
        }

        public async Task<List<Course>?> FilterCourseInformationByNameAsync(string name)
        {
            var courseFiltered = await context.Courses.Where(u => u.CourseName.Contains(name)).ToListAsync();
            if (courseFiltered.Count == 0) return null;
            return courseFiltered;
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            var course = await context.Courses.FirstOrDefaultAsync(u => u.CourseId == id);
            if (course is null) return null;
            return course;
        }

        public async Task<Course?> GetCourseDetailAsync(int id )
        {
            var CourseDetail = await context.Courses
                .Include(u => u.CourseSemester)
                    .ThenInclude(cs => cs.Semester)
                .FirstOrDefaultAsync(u => u.CourseId == id);
            return CourseDetail;
        }
    }
}
