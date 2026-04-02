using AutoMapper;
using SchoolManagement.DTOs.Gpa;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class GpaService(IUnitOfWork uow, ILogger<GpaService> logger) : IGpaService
    {
        public async Task<List<GpaResponse>> GetAllGpas()
        {
            using (logger.BeginOperationScope("GetAllGpas"))
            using (var timer = logger.TimeOperation("GetAllGpas"))
            {
                try
                {
                    return await uow.Gpa.GetAllGpaAsync();
                }catch(Exception e)
                {
                    logger.LogOperationError("GetAllGpa", e);
                    throw;
                }
            }
        }
    }
}
