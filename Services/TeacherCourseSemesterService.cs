using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class TeacherCourseSemesterService(IUnitOfWork uow, ITeacherCourseSemesterRepository repository, IMapper mapper) : ITeacherCourseSemesterService
    {
        public async Task<TeacherCourseSemesterResponse> AllocateTeacherToCourse(AllocateTeacherCourseSemesterRequest request)
        {
            if (!await uow.CourseSemester.CheckValidCurrentId(request.CourseSemesterId)) throw new BadRequestException($"The given Course-Semester ID {request.CourseSemesterId} is not valid");
            if (!await uow.Users.IsTeacherAsync(request.TeacherId)) throw new BadRequestException($"The given Teacher ID {request.TeacherId} is not valid");
            
            var teacherCourseSemester = new TeacherCourseSemester { CourseSemesterId = request.CourseSemesterId, TeacherId = request.TeacherId };
            await repository.AllocateTeacherToCourseAsync(teacherCourseSemester);
            await uow.SaveChangeAsync();
            var newTeacherCourseSemester = await uow.TeacherCourseSemester.GetTeacherCourseSemesterByIdAsync(teacherCourseSemester.TeacherCourseSemesterId);
            var newTeacherCourseSemesterResponse = mapper.Map<TeacherCourseSemesterResponse>(newTeacherCourseSemester);
            return newTeacherCourseSemesterResponse;
        }

        public async Task DeleteTeacherFromCourse(int id)
        {
            var result = await uow.TeacherCourseSemester.GetTeacherCourseSemesterByIdAsync(id);
            if (result is null) throw new NotFoundException($"The ID {id} was not found");
            await uow.TeacherCourseSemester.DeleteTeacherFromCourse(result);
            await uow.SaveChangeAsync();
        }

        public async Task<List<TeacherCourseSemesterResponse>?> GetAllTeacherCourseSemester()
        {
            var teacherCourseSemester = await repository.GetAllTeacherCourseSemesterAsync();
            if (teacherCourseSemester is null) throw new BadRequestException("Failed to load datas");
            var listResult = teacherCourseSemester.Select(u => mapper.Map<TeacherCourseSemesterResponse>(u)).ToList();
            return listResult;
        }
    }
}
