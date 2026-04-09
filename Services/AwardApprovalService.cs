using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace SchoolManagement.Services
{
    public class AwardApprovalService(IUnitOfWork uow, IMapper mapper, ILogger<AwardApprovalService> logger) : IAwardApprovalService
    {
        public async Task<AwardApprovalResponse> CreateAwardApproval(CreateAwardApprovalRequest request)
        {
            using (logger.BeginOperationScope("CreateAwardApproval", ("AwardId", request.AwardId), ("TeacherId", request.TeacherId)))
            using (var timer = logger.TimeOperation("CreateAwardApproval"))
            {
                if (await uow.AwardApproval.ExistsAsync(u => u.AwardId == request.AwardId && u.TeacherId == request.TeacherId)) throw new ConflictException("Award Id and Teacher Id is already existed");
                if (!await uow.Award.ExistsAsync(u => u.AwardId == request.AwardId)) throw new NotFoundException($"The given AwardId {request.AwardId} was not found");
                if (!await uow.User.ExistsAsync(u => u.UserId == request.TeacherId)) throw new NotFoundException($"The given TeacherId {request.TeacherId} was not found");

                if (!await uow.User.IsTeacherAsync(request.TeacherId)) throw new BadRequestException($"The given Id {request.TeacherId} was not owned by a Teacher");
                try
                {
                    var approval = mapper.Map<AwardApproval>(request);
                    var result = await uow.AwardApproval.CreateApprovalAsync(approval);
                    await uow.SaveChangeAsync();
                    var response = await uow.AwardApproval.GetAwardApprovalResponseViaIdAsync(result.ApprovalId);
                    logger.LogEntityCreated<AwardApproval>("AwardApproval", response!.ApprovalId);
                    return response!;
                }
                catch(Exception e)
                {
                    logger.LogOperationError("CreateAwardApproval", e, request.AwardId, request.TeacherId);
                    throw;
                }

            }
        }

        public async Task DeleteAwardApproval(int id)
        {
            using (logger.BeginOperationScope("DeleteAwardApproval", ("AwardApprovalID", id)))
            using (var timer = logger.TimeOperation("DeleteAwardApproval"))
            {
                var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(id);
                if (approval is null) throw new NotFoundException($"The given AwardApprovalId {id} was not found");
                try
                {
                    await uow.AwardApproval.DeleteAwardApprovalAsync(approval);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted<AwardApproval>("AwardApproval", id);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteAwardApproval", e, id);
                    throw;
                }
            }
        }

        public async Task<List<AwardApprovalResponse>> GetAllAwardApprovals()
        {
            using (logger.BeginOperationScope("GetAllAwardApprovals"))
            using (var timer = logger.TimeOperation("GetAllAwardApprovals"))
            {
                try
                {
                    var approvals =  await uow.AwardApproval.GetAllApprovalsAsync();
                    logger.LogInformation("Retrieved {Count} approvals", approvals.Count);
                    return approvals;
                }catch(Exception e)
                {
                    logger.LogOperationError("GetAllAwardApproval", e);
                    throw;
                }
            }
        }

        public async Task<List<AwardApprovalResponse>> GetAwardApprovalsViaTeacherId(int teacherId)
        {
            using (logger.BeginOperationScope("GetAwardApprovalsViaTeacherId", ("TeacherId", teacherId)))
            using (var timer = logger.TimeOperation("GetAwardApprovalsViaTeacherId"))
            {
                try
                {
                    return await uow.AwardApproval.GetListApprovalsViaTeacherIdAsync(teacherId);
                }catch (Exception e)
                {
                    logger.LogOperationError("GetAwardApprovalsViaTeacherId", e);
                    throw;
                }
            }
        }

        public async Task UpdateAwardApproval(int id, UpdateAwardApprovalRequest request)
        {
            using (logger.BeginOperationScope("UpdateAwardApproval", ("AwardApprovalID", id)))
            using (var timer = logger.TimeOperation("UpdateAwardApproval"))
            {
                logger.LogInformation("Update method for Admin");
                var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(id);
                if (approval is null) throw new NotFoundException($"The given AwardApprovalId {id} was not found");
                var award = await uow.Award.GetAwardViaId(approval.AwardId);
                if (request.decision != null)
                {
                    switch (request.decision.ToLower())
                    {
                        case "approve":
                            approval.decision = Decision.Approve;
                            break;
                        case "reject":
                            approval.decision = Decision.Reject;
                            break;
                        default:
                            throw new BadRequestException("Invalid input");
                    }
                }
                approval.DecisionDate = DateTime.UtcNow;
                if (approval.DecisionDate > award!.ExpiredDate) throw new BadRequestException("The period allocated for decision-making has ended");
                if (request.Comment != "") approval.Comment = request.Comment;
                await uow.AwardApproval.UpdateAwardApprovalAsync(approval);
                await uow.SaveChangeAsync();

                int approvalsNum = await uow.AwardApproval.CountApprovedAwardApprovalsByAwardId(approval.AwardId);
                await uow.Award.CheckRequireApprovalsAsync(award, approvalsNum);
                await uow.SaveChangeAsync();
                logger.LogEntityUpdated<AwardApproval>("AwardApproval", id);
            }
        }
        public async Task UpdateAwardApprovalForTeacher(int id,int teacherId, UpdateAwardApprovalRequest request)
        {
            using (logger.BeginOperationScope("UpdateAwardApproval", ("AwardApprovalID", id)))
            using (var timer = logger.TimeOperation("UpdateAwardApproval"))
            {
                logger.LogInformation("Update method for Teacher");
                var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(id);
                if (approval is null) throw new NotFoundException($"The given AwardApprovalId {id} was not found");
                try
                {
                    var award = await uow.Award.GetAwardViaId(approval.AwardId);
                    if (approval.decision != null) throw new BadRequestException("You cannot change your decision");
                    if (approval.TeacherId != teacherId) throw new ForbiddenException("You can only update approvals assigned to you");

                    if (request.decision != null)
                    {
                        switch (request.decision.ToLower())
                        {
                            case "approve":
                                approval.decision = Decision.Approve;
                                break;
                            case "reject":
                                approval.decision = Decision.Reject;
                                break;
                            default:
                                throw new BadRequestException("Invalid input, it must be 'Approve' or 'Reject' ");
                        }
                    }
                    approval.DecisionDate = DateTime.UtcNow;
                    if (approval.DecisionDate > award!.ExpiredDate) throw new BadRequestException("The period allocated for decision-making has ended");
                    if (request.Comment != "") approval.Comment = request.Comment;
                    await uow.AwardApproval.UpdateAwardApprovalAsync(approval);
                    await uow.SaveChangeAsync();

                    int approvalsNum = await uow.AwardApproval.CountApprovedAwardApprovalsByAwardId(approval.AwardId);
                    await uow.Award.CheckRequireApprovalsAsync(award, approvalsNum);
                    await uow.SaveChangeAsync();
                    logger.LogEntityUpdated<AwardApproval>("AwardApproval", id);
                }catch(Exception e)
                {
                    logger.LogOperationError("UpdateAwardApproval", e, teacherId, id);
                    throw;
                }
            }
        }
    }
}
