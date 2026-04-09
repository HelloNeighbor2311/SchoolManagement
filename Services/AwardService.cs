using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.DTOs.Award;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class AwardService(IUnitOfWork uow, IMapper mapper, ILogger<AwardService> logger) : IAwardService
    {
        public async Task<AwardResponse> CreateAward(CreateAwardRequest request)
        {
            using (logger.BeginOperationScope("CreateAward", ("GpaId", request.GpaId)))
            using (var timer = logger.TimeOperation("CreateAward"))
            {
                if (!await uow.Gpa.ExistsAsync(p => p.GPAId == request.GpaId)) throw new NotFoundException($"The Gpa with the id {request.GpaId} was not found");
                try
                {
                    var Gpa = await uow.Gpa.FindGpaViaIdAsync(request.GpaId);
                    var award = mapper.Map<Award>(request);
                    var result = await uow.Award.CreateAwardAsync(award);
                    result.StudentId = Gpa.StudentId;
                    await uow.SaveChangeAsync();
                    logger.LogEntityCreated("Award", result.AwardId);
                    var response = await uow.Award.GetAwardResponseViaId(result.AwardId);
                    return response!;
                }catch(Exception e) {
                    logger.LogOperationError("CreateAward", e, request.GpaId, request.Description, request.RequireApproval, request.Description, request.ExpiredDate);
                    throw;
                }
            }
        }

        public async Task DeleteAward(int awardId)
        {
            using (logger.BeginOperationScope("Deleteward", ("AwardId", awardId)))
            using (var timer = logger.TimeOperation("DeleteAward"))
            {
                var award = await uow.Award.GetAwardViaId(awardId);
                if (award is null) throw new NotFoundException($"The Award with the given id {awardId} was not found");
                try
                {
                    await uow.Award.DeleteAwardAsync(award);
                    await uow.SaveChangeAsync();
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteAward", e, awardId);
                    throw;
                }
            }
        }

        public async Task<List<AwardResponse>> GetAllAwards()
        {
            using (logger.BeginOperationScope("GetAllAwards"))
            using (var timer = logger.TimeOperation("GetAllAwards"))
            {
                try
                {
                    var awards = await uow.Award.GetAllAwardsAsync();
                    logger.LogInformation("Retrieved {Count} Awards", awards.Count);
                    return awards;
                }
                catch (Exception e)
                {
                    logger.LogOperationError("GetAllAwards", e);
                    throw;
                }
            }
        }

        public async Task<AwardResponse> UpdateAward(int awardId, UpdateAwardRequest request)
        {
            using (logger.BeginOperationScope("UpdateAward", ("AwardId", awardId)))
            using (var timer = logger.TimeOperation("UpdateAward"))
            {
                var award = await uow.Award.GetAwardViaId(awardId);
                if (award is null) throw new NotFoundException($"The given AwardId {awardId} was not found");
                var rowVersionBytes = Convert.FromBase64String(request.RowVersion);
                uow.Award.SetRowVersion(award, rowVersionBytes);
                if (!string.IsNullOrWhiteSpace(request.Description)) award.Description = request.Description;
                if (request.RequireApproval != null) award.RequireApproval = request.RequireApproval;
                award.status = request.status.ToLower() switch
                {
                    "approved" => Status.Approved,
                    "rejected" => Status.Rejected,
                    _ => Status.Pending
                };
                try
                {
                    await uow.SaveChangeAsync();
                    return await uow.Award.GetAwardResponseViaId(awardId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw new ConflictException("Award has been modified by someone. Please try again later");
                }
            }
        }
    }
}
