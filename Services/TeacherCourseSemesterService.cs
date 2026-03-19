using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SchoolManagement.DTOs.TeacherCourseSemester;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories.Interfaces;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class TeacherCourseSemesterService(IUnitOfWork uow, ITeacherCourseSemesterRepository repository, IMapper mapper) : ITeacherCourseSemesterService
    {
        //public Task<TeacherCourseSemesterResponse> AllocateTeacherToCourse(AllocateTeacherCourseSemesterRequest request)
        //{
        //    if()
        //}

        public async Task<List<TeacherCourseSemesterResponse>?> GetAllTeacherCourseSemester()
        {
            var teacherCourseSemester = await repository.GetAllTeacherCourseSemesterAsync();
            if (teacherCourseSemester is null) throw new BadRequestException("Failed to load datas");
            var listResult = teacherCourseSemester.Select(u => mapper.Map<TeacherCourseSemesterResponse>(u)).ToList();
            return listResult;
        }
    }
}
