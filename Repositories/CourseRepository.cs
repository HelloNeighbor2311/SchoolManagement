using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Course;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using System.Collections;

namespace SchoolManagement.Repositories
{
    public class CourseRepository(AppDbContext context, IMapper mapper) : ICourseRepository
    {
        public async Task<Course?> CreateCourseAsync(Course request)
        {
            if (await context.Courses.AnyAsync(p => p.CourseName == request.CourseName)) return null;
            await context.Courses.AddAsync(request);
            return request;
        }

        public async Task DeleteCourseAsync(Course course)
        {
            context.Courses.Remove(course);
        }

        public async Task<List<CourseResponse>> GetAllCourseAsync()
        {
            var courses = await context.Courses.ProjectTo<CourseResponse>(mapper.ConfigurationProvider).ToListAsync();
            return courses;
        }

        public async Task<List<CourseResponse>?> FilterCourseInformationByNameAsync(string name)
        {
            var courseFiltered = await context.Courses.Where(u => u.CourseName.Contains(name))
            .ProjectTo<CourseResponse>(mapper.ConfigurationProvider).ToListAsync();
            if (courseFiltered.Count == 0) return null;
            return courseFiltered;
        }

        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            var course = await context.Courses.FirstOrDefaultAsync(u => u.CourseId == courseId);
            if (course is null) return null;
            return course;
        }

        public async Task<CourseDetailResponse?> GetCourseDetailAsync(int courseId )
        {
            var CourseDetail = await context.Courses
                .Include(u => u.CourseSemester)
                    .ThenInclude(cs => cs.Semester).ProjectTo<CourseDetailResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.CourseId == courseId);
                return CourseDetail;
        }

        public async Task<CourseResponse?> GetCourseResponseByIdAsync(int courseId)
        {
            return await context.Courses.ProjectTo<CourseDetailResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.CourseId == courseId);
        }

    }
}
