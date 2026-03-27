using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.DTOs.Award;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class AwardService(IUnitOfWork uow, IMapper mapper) : IAwardService
    {
        public async Task<AwardResponse> CreateAward(CreateAwardRequest request)
        {
            if (!await uow.Gpa.ExistsAsync(p => p.GPAId == request.GpaId)) throw new NotFoundException($"The Gpa with the id {request.GpaId} was not found");
            var Gpa = await uow.Gpa.FindGpaViaIdAsync(request.GpaId);
            var award = mapper.Map<Award>(request);
            var result = await uow.Award.CreateAwardAsync(award);
            result.StudentId = Gpa.StudentId;
            await uow.SaveChangeAsync();
            var response = await uow.Award.GetAwardResponseViaId(result.AwardId);
            return response!;
        }

        public async Task DeleteAward(int id)
        {
            var award = await uow.Award.GetAwardViaId(id);
            if (award is null) throw new NotFoundException($"The Award with the given id {id} was not found");
            await uow.Award.DeleteAwardAsync(award);
            await uow.SaveChangeAsync();
        }

        public async Task<List<AwardResponse>> GetAllAwards()
        {
            return await uow.Award.GetAllAwardsAsync();
        }

        public async Task<AwardResponse> UpdateAward(int id, UpdateAwardRequest request)
        {
            var award = await uow.Award.GetAwardViaId(id);
            if (award is null) throw new NotFoundException($"The given AwardId {id} was not found");
            var rowVersionBytes = Convert.FromBase64String(request.RowVersion);
            uow.Award.SetRowVersion(award, rowVersionBytes);
            if(!string.IsNullOrWhiteSpace(award.Description)) award.Description = request.Description;
            if(request.RequireApproval!=null) award.RequireApproval = request.RequireApproval;
            award.status = request.status.ToLower() switch
            {
                "approved" => Status.Approved,
                "rejected" => Status.Rejected,
                _=> Status.Pending
            };
            try { 
                await uow.SaveChangeAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException(
                    "Award has been modified by someone. Please try again later");
            }
            return await uow.Award.GetAwardResponseViaId(id);
        }
    }
}
