using AutoMapper;
using SchoolManagement.DTOs.Semester;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class SemesterService(IMapper mapper, IUnitOfWork uow) : ISemesterService
    {
        public async Task<SemesterResponse> CreateSemester(CreateSemesterRequest request)
        {
            var semester = mapper.Map<Semester>(request);
            var addedSemester = await uow.Semester.CreateSemesterAsync(semester);
            await uow.SaveChangeAsync();
            var savedSemester = mapper.Map<SemesterResponse>(addedSemester);
            return savedSemester;
        }

        public async Task DeleteSemester(int id)
        {
            var semester = await uow.Semester.GetSemesterByIdAsync(id);
            if (semester is null) throw new NotFoundException($"The semester with the given id {id} was not found !");
            await uow.Semester.DeleteSemesterAsync(semester);
            await uow.SaveChangeAsync();
        }

        public async Task<List<SemesterResponse>> GetAllSemester()
        {
            var semester = await uow.Semester.GetAllSemesterAsync();
            await uow.SaveChangeAsync();
            var semesterResponse = semester.Select(u => mapper.Map<SemesterResponse>(u)).ToList();
            return semesterResponse;
        }

        public async Task<SemesterResponse> GetSemesterById(int id)
        {
            var semester = await uow.Semester.GetSemesterByIdAsync(id);
            if (semester is null) throw new NotFoundException($"The semester with the given id {id} was not found !");
            var semesterResponse = mapper.Map<SemesterResponse>(semester);
            return semesterResponse;
        }

        public async Task<SemesterDetailResponse> GetSemesterDetail(int id)
        {
            var semester = await uow.Semester.GetSemesterDetailAsync(id);
            if (semester is null) throw new NotFoundException($"The semester with the given id {id} was not found");
            var semesterDetailResponse = mapper.Map<SemesterDetailResponse>(semester);
            return semesterDetailResponse;
        }
    }
}
