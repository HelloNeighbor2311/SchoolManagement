using AutoMapper;
using SchoolManagement.DTOs.Gpa;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class GpaService(IUnitOfWork uow) : IGpaService
    {
        public async Task<List<GpaResponse>> GetAllGpas()
        {
            return await uow.Gpa.GetAllGpaAsync();
        }
    }
}
