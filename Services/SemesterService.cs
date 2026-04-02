using AutoMapper;
using SchoolManagement.DTOs.Semester;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;
using Serilog.Core;

namespace SchoolManagement.Services
{
    public class SemesterService(IMapper mapper, IUnitOfWork uow, ILogger<SemesterService> logger) : ISemesterService
    {
        public async Task<SemesterResponse> CreateSemester(CreateSemesterRequest request)
        {
            using (logger.BeginOperationScope("CreateSemester"))
            using (var timer = logger.TimeOperation("CreateSemester"))
            {
                var semester = mapper.Map<Semester>(request);
                var addedSemester = await uow.Semester.CreateSemesterAsync(semester);
                try
                {
                    await uow.SaveChangeAsync();
                    var savedSemester = mapper.Map<SemesterResponse>(addedSemester);
                    logger.LogEntityCreated<Semester>("Semester", savedSemester.SemesterId);
                    return savedSemester;
                }catch(Exception e)
                {
                    logger.LogOperationError("CreateSemester", e);
                    throw;
                }
            }
        }

        public async Task DeleteSemester(int id)
        {
            using (logger.BeginOperationScope("DeleteSemester", ("SemesterId", id)))
            using (var timer = logger.TimeOperation("DeleteSemester"))
            {
                var semester = await uow.Semester.GetSemesterByIdAsync(id);
                if (semester is null) throw new NotFoundException($"The semester with the given id {id} was not found !");
                try
                {
                    await uow.Semester.DeleteSemesterAsync(semester);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted<Semester>("Semester", id);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteSemester", e, id);
                    throw;
                }
            }
        }

        public async Task<List<SemesterResponse>> GetAllSemester()
        {
            using (logger.BeginOperationScope("GetAllSemester"))
            using (var timer = logger.TimeOperation("GetAllSemester"))
            {
                var semester = await uow.Semester.GetAllSemesterAsync();
                await uow.SaveChangeAsync();
                var semesterResponse = semester.Select(u => mapper.Map<SemesterResponse>(u)).ToList();
                return semesterResponse;
            }
        }

        public async Task<SemesterResponse> GetSemesterById(int id)
        {
            using (logger.BeginOperationScope("GetSemesterById", ("SemesterId", id)))
            using (var timer = logger.TimeOperation("GetSemesterById"))
            {
                var semester = await uow.Semester.GetSemesterByIdAsync(id);
                if (semester is null) throw new NotFoundException($"The semester with the given id {id} was not found !");
                var semesterResponse = mapper.Map<SemesterResponse>(semester);
                return semesterResponse;
            }
        }

        public async Task<SemesterDetailResponse> GetSemesterDetail(int id)
        {
            using (logger.BeginOperationScope("GetSemesterDetail", ("SemesterId", id)))
            using (var timer = logger.TimeOperation("GetSemesterDetail"))
            {
                var semester = await uow.Semester.GetSemesterDetailAsync(id);
                if (semester is null) throw new NotFoundException($"The semester with the given id {id} was not found");
                var semesterDetailResponse = mapper.Map<SemesterDetailResponse>(semester);
                return semesterDetailResponse;
            }
        }
    }
}
