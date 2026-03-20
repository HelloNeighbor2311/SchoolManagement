using AutoMapper;
using SchoolManagement.DTOs.Grade;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class GradeService(IUnitOfWork uow, IMapper mapper) : IGradeService
    {
        public async Task<List<GradeResponse>> GetGradeWithStudentId(int id)
        {
            if (!await uow.User.IsStudentAsync(id)) throw new ConflictException($"The user with the given Id {id} was not a student");
            var result = await uow.Grade.GetAllGradeWithStudentIdAsync(id);
            var newGrade = result.Select(u => mapper.Map<GradeResponse>(u)).ToList();
            return newGrade;
        }
    }
}
