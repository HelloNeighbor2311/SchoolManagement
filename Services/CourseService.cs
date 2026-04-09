using AutoMapper;
using Microsoft.Identity.Client;
using SchoolManagement.DTOs.Course;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class CourseService(IMapper mapper, IUnitOfWork uow, ILogger<CourseService> logger) : ICourseService
    {

        public async Task<CourseResponse> CreateCourse(CreateCourseRequest request)
        {
            using (logger.BeginOperationScope("CreateCourse"))
            using (var timer = logger.TimeOperation("CreateCourse"))
            {
                try
                {
                    var course = mapper.Map<Course>(request);
                    var newCourse = await uow.Course.CreateCourseAsync(course);
                    if (newCourse is null) throw new BadRequestException("The current course name is already existed !");
                    await uow.SaveChangeAsync();
                    var response = mapper.Map<CourseResponse>(newCourse);
                    logger.LogEntityCreated("Course", response.CourseId);
                    return response;
                }catch (Exception e)
                {
                    logger.LogOperationError("CreateCourse", e);
                    throw;
                }
            }
        }

        public async Task DeleteCourse(int courseId)
        {
            using (logger.BeginOperationScope("DeleteCourse", ("CourseId", courseId)))
            using (var timer = logger.TimeOperation("DeleteCourse"))
            {
                var course = await uow.Course.GetCourseByIdAsync(courseId);
                if (course is null) throw new NotFoundException($"The given id {courseId} is not existed!");
                try
                {
                    await uow.Course.DeleteCourseAsync(course);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted("Course", courseId);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteCourse", e, courseId);
                    throw;
                }
            }
        }

        public async Task<List<CourseResponse>> FilterCourseInformationByName(string name)
        {
            using (logger.BeginOperationScope("FilterCourseInformationByName", ("CourseName", name)))
            using (var timer = logger.TimeOperation("FilterCourseInformationByName"))
            {
                var listCourse = await uow.Course.FilterCourseInformationByNameAsync(name);
                if (listCourse is null) throw new NotFoundException($"There is no course meets the '{name}'");
                try
                {
                    var listCourseResponse = listCourse.Select(u => mapper.Map<CourseResponse>(u)).ToList();
                    return listCourseResponse;
                }catch(Exception e)
                {
                    logger.LogOperationError("FilterCourseInformationByName", e, name);
                    throw;
                }
            }
        }

        public async Task<List<CourseResponse>> GetAllCourse()
        {
            using (logger.BeginOperationScope("GetAllCourse"))
            using (var timer = logger.TimeOperation("GetAllCourse"))
            {
                try
                {
                    var course = await uow.Course.GetAllCourseAsync();
                    var courseResponse = course.Select(u => mapper.Map<CourseResponse>(u)).ToList();
                    return courseResponse;
                }catch(Exception e)
                {
                    logger.LogOperationError("GetAllCourse", e);
                    throw;
                }
            }
        }

        public async Task<CourseResponse> GetCourseById(int courseId)
        {
            using (logger.BeginOperationScope("GetCourseById", ("CourseId", courseId)))
            using (var timer = logger.TimeOperation("GetCourseById"))
            {
                try
                {
                    var course = await uow.Course.GetCourseByIdAsync(courseId);
                    if (course is null) throw new NotFoundException($"The given id {courseId} is not existed!");
                    var courseResponse = mapper.Map<CourseResponse>(course);
                    return courseResponse;
                }
                catch (Exception e)
                {
                    logger.LogOperationError("GetCourseById", e, courseId);
                    throw;
                }
            }
        }

        public async Task<CourseDetailResponse?> GetCourseDetail(int courseId)
        {
            using (logger.BeginOperationScope("GetCourseDetail", ("CourseId", courseId)))
            using (var timer = logger.TimeOperation("GetCourseDetail"))
            {
                try
                {
                    var course = await uow.Course.GetCourseDetailAsync(courseId);
                    if (course is null) throw new NotFoundException($"The course with the given id {courseId} was not found");
                    var CourseDetailResponse = mapper.Map<CourseDetailResponse>(course);
                    return CourseDetailResponse;
                }catch(Exception e)
                {
                    logger.LogOperationError("GetCourseDetail", e, courseId);
                    throw;
                }
            }

        }
    }
}
