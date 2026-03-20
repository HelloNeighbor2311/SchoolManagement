using AutoMapper;
using Microsoft.Identity.Client;
using SchoolManagement.DTOs.Course;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class CourseService(IMapper mapper, IUnitOfWork uow) : ICourseService
    {
        public async Task<CourseResponse> CreateCourse(CreateCourseRequest request)
        {
            var course = mapper.Map<Course>(request);
            var newCourse = await uow.Course.CreateCourseAsync(course);
            if (newCourse is null) throw new BadRequestException("The current course name is already existed !");
            await uow.SaveChangeAsync();
            return mapper.Map<CourseResponse>(newCourse);
        }

        public async Task DeleteCourse(int id)
        {
            var course = await uow.Course.GetCourseByIdAsync(id);
            if (course is null) throw new NotFoundException($"The given id {id} is not existed!");
            await uow.Course.DeleteCourseAsync(course);
            await uow.SaveChangeAsync();
        }

        public async Task<List<CourseResponse>> FilterCourseInformationByName(string name)
        {
            var listCourse = await uow.Course.FilterCourseInformationByNameAsync(name);
            if (listCourse is null) throw new NotFoundException($"There is no course meets the '{name}'");
            var listCourseResponse = listCourse.Select(u => mapper.Map<CourseResponse>(u)).ToList();
            return listCourseResponse;
        }

        public async Task<List<CourseResponse>> GetAllCourse()
        {
            var course = await uow.Course.GetAllCourseAsync();
            var courseResponse = course.Select(u => mapper.Map<CourseResponse>(u)).ToList();
            return courseResponse;
        }

        public async Task<CourseResponse> GetCourseById(int id)
        {
            var course = await uow.Course.GetCourseByIdAsync(id);
            if (course is null) throw new NotFoundException($"The given id {id} is not existed!");
            var courseResponse = mapper.Map<CourseResponse>(course);
            return courseResponse;
        }

        public async Task<CourseDetailResponse?> GetCourseDetail(int id)
        {
            var course = await uow.Course.GetCourseDetailAsync(id);
            if (course is null) throw new NotFoundException($"The course with the given id {id} was not found");
            var CourseDetailResponse = mapper.Map<CourseDetailResponse>(course);
            return CourseDetailResponse;

        }
    }
}
