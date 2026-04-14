using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class TeacherCourseSemesterService(IUnitOfWork uow, IMapper mapper, ILogger<TeacherCourseSemester> logger) : ITeacherCourseSemesterService
    {
        public async Task<TeacherCourseSemesterResponse> AllocateTeacherToCourse(AllocateTeacherCourseSemesterRequest request)
        {
            using (logger.BeginOperationScope("AllocateTeacherToCourse", ("TeacherId", request.TeacherId), ("CourseSemesterId", request.CourseSemesterId)))
            using (var timer = logger.TimeOperation("GetAllSemester"))
            {
                if (!await uow.CourseSemester.ExistsAsync(p => p.CourseSemesterId == request.CourseSemesterId)) throw new BadRequestException($"The given Course-Semester ID {request.CourseSemesterId} is not valid");
                if (!await uow.User.IsTeacherAsync(request.TeacherId)) throw new BadRequestException($"The given Teacher ID {request.TeacherId} is not valid");
                bool isDuplicated = await uow.TeacherCourseSemester.ExistsAsync(p => p.CourseSemesterId == request.CourseSemesterId && p.TeacherId == request.TeacherId);
                if (isDuplicated) throw new ConflictException($"The teacher with the given Id {request.TeacherId} is already assgined to Course with the Id {request.CourseSemesterId}");
                try
                {
                    var teacherCourseSemester = new TeacherCourseSemester
                    {
                        TeacherId = request.TeacherId,
                        CourseSemesterId = request.CourseSemesterId
                    };
                    await uow.TeacherCourseSemester.AllocateTeacherToCourseAsync(teacherCourseSemester);
                    await uow.SaveChangeAsync();
                    var newTeacherCourseSemester = await uow.TeacherCourseSemester.GetTeacherCourseSemesterByIdAsync(teacherCourseSemester.TeacherCourseSemesterId);
                    var newTeacherCourseSemesterResponse = mapper.Map<TeacherCourseSemesterResponse>(newTeacherCourseSemester);
                    logger.LogEntityCreated("TeacherCourseSemester", newTeacherCourseSemesterResponse.TeacherCourseSemesterId);
                    return newTeacherCourseSemesterResponse;
                }catch(Exception e)
                {
                    logger.LogOperationError("AllocateTeacherToCourse", e, request.TeacherId, request.CourseSemesterId);
                    throw;
                }
            }
        }

        public async Task DeleteTeacherFromCourse(int teacherCourseSemesterId)
        {
            using (logger.BeginOperationScope("DeleteTeacherFromCourse", ("TeacherCourseSemesterId", teacherCourseSemesterId)))
            using (var timer = logger.TimeOperation("DeleteTeacherFromCourse"))
            {
                var result = await uow.TeacherCourseSemester.GetTeacherCourseSemesterByIdAsync(teacherCourseSemesterId);
                if (result is null) throw new NotFoundException($"The ID {teacherCourseSemesterId} was not found");
                await uow.TeacherCourseSemester.DeleteTeacherFromCourse(result);
                try
                {
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted("TeacherCourseSemester", teacherCourseSemesterId);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteTeacherFromCourse", e, teacherCourseSemesterId);
                    throw;
                }
            }
        }

        public async Task<List<TeacherCourseSemesterResponse>?> GetAllTeacherCourseSemester()
        {
            using (logger.BeginOperationScope("GetAllTeacherCourseSemester"))
            using (var timer = logger.TimeOperation("GetAllTeacherCourseSemester"))
            {
                var teacherCourseSemester = await uow.TeacherCourseSemester.GetAllTeacherCourseSemesterAsync();
                if (teacherCourseSemester is null) throw new BadRequestException("Failed to load datas");
                var listResult = teacherCourseSemester.Select(u => mapper.Map<TeacherCourseSemesterResponse>(u)).ToList();
                return listResult;
            }
        }
    }
}
