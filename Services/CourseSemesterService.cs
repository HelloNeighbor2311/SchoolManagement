using AutoMapper;
using Azure.Core;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class CourseSemesterService(IUnitOfWork uow, IMapper mapper, ILogger<CourseSemesterService> logger) : ICourseSemesterService
    {
        public async Task<CourseSemesterResponse> CreateCourseSemester(CreateCourseSemesterRequest request)
        {
            using (logger.BeginOperationScope("CreateCourseSemester", ("CourseId", request.CourseId), ("SemesterId", request.SemesterId)))
            using (var timer = logger.TimeOperation("CreateCourseSemester"))
            {
                if (await uow.Course.GetCourseByIdAsync(request.CourseId) == null) throw new NotFoundException($"The Course with the Id {request.CourseId} was not found !");
                if (await uow.Semester.GetSemesterByIdAsync(request.SemesterId) is null) throw new NotFoundException($"The Semester with the Id {request.SemesterId} was not found !");
                var isDuplicated = await uow.CourseSemester.ExistsAsync(p => p.CourseId == request.CourseId && p.SemesterId == request.SemesterId);
                if (isDuplicated) throw new ConflictException($"The Course with the Id {request.CourseId} is already existed in Semester with the Id {request.SemesterId}");
                var courseSemester = mapper.Map<CourseSemester>(request);
                try
                {
                    await uow.CourseSemester.CreateCourseSemesterAsync(courseSemester);
                    await uow.SaveChangeAsync();
                    var newCourseSemester = await uow.CourseSemester.GetCourseSemesterByIdAsync(courseSemester.CourseSemesterId);
                    var courseSemesterResponse = mapper.Map<CourseSemesterResponse>(newCourseSemester);
                    logger.LogEntityCreated("CourseSemester", courseSemesterResponse.CourseSemesterId);
                    return courseSemesterResponse;
                }catch(Exception ex)
                {
                    logger.LogOperationError("CreateCourseSemester", ex, courseSemester.CourseId, courseSemester.SemesterId);
                    throw;
                }
            }
        }

        public async Task DeleteCourseSemester(int courseSemesterId)
        {
            using (logger.BeginOperationScope("DeleteCourseSemester", ("CourseSemesterId", courseSemesterId)))
            using (var timer = logger.TimeOperation("DeleteCourseSemester"))
            {
                if (!await uow.CourseSemester.ExistsAsync(p => p.CourseSemesterId == courseSemesterId)) throw new NotFoundException($"The Course Semester with the Id {courseSemesterId} was not found");
                var courseSemester = await uow.CourseSemester.GetCourseSemesterByIdAsync(courseSemesterId);
                try
                {
                    await uow.CourseSemester.DeletetCourseSemesterAsync(courseSemester!);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted("CourseSemester", courseSemesterId);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteCourseSemester", e, courseSemesterId);
                }
            }
        }
    }
}
