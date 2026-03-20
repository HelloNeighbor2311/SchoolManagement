using AutoMapper;
using SchoolManagement.DTOs.CourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class CourseSemesterService(IUnitOfWork uow, IMapper mapper) : ICourseSemesterService
    {
        public async Task<CourseSemesterResponse> CreateCourseSemester(CreateCourseSemesterRequest request)
        {
            if (await uow.Course.GetCourseByIdAsync(request.CourseId) == null) throw new NotFoundException($"The Course with the Id {request.CourseId} was not found !");
            if (await uow.Semester.GetSemesterByIdAsync(request.SemesterId) is null) throw new NotFoundException($"The Semester with the Id {request.SemesterId} was not found !");
            var isDuplicated = await uow.CourseSemester.ExistsAsync(p => p.CourseId == request.CourseId && p.SemesterId == request.SemesterId);
            if (isDuplicated) throw new ConflictException($"The Course with the Id {request.CourseId} is already existed in Semester with the Id {request.SemesterId}");
            var courseSemester = mapper.Map<CourseSemester>(request);
            await uow.CourseSemester.CreateCourseSemesterAsync(courseSemester);
            await uow.SaveChangeAsync();
            var newCourseSemester = await uow.CourseSemester.GetCourseSemesterByIdAsync(courseSemester.CourseSemesterId);
            var courseSemesterResponse = mapper.Map<CourseSemesterResponse>(newCourseSemester);
            return courseSemesterResponse;
        }

        public async Task DeleteCourseSemester(int id)
        {
            if (!await uow.CourseSemester.ExistsAsync(p => p.CourseSemesterId == id)) throw new NotFoundException($"The Course Semester with the Id {id} was not found");
            var courseSemester = await uow.CourseSemester.GetCourseSemesterByIdAsync(id);
            await uow.CourseSemester.DeletetCourseSemesterAsync(courseSemester!);
            await uow.SaveChangeAsync();
        }
    }
}
