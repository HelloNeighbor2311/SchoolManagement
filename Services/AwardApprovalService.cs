using Microsoft.Extensions.Logging;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Exceptions;
using SchoolManagement.Infrastructure.Logging;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class AwardApprovalService(IUnitOfWork uow, ILogger<AwardApprovalService> logger) : IAwardApprovalService
    {
        public async Task<AwardApprovalResponse> CreateAwardApproval(CreateAwardApprovalRequest request)
        {
            using (logger.BeginOperationScope("CreateAwardApproval", ("AwardId", request.AwardId), ("TeacherId", request.TeacherId)))
            using (var timer = logger.TimeOperation("CreateAwardApproval"))
            {
                await ValidateCreateRequestAsync(request);
                try
                {
                    var approval = new AwardApproval
                    {
                        AwardId = request.AwardId,
                        TeacherId = request.TeacherId,
                        Comment = request.Comment,
                        decision = null,
                        DecisionDate = null
                    };

                    var result = await uow.AwardApproval.CreateApprovalAsync(approval);
                    await uow.SaveChangeAsync();
                    var response = await uow.AwardApproval.GetAwardApprovalResponseViaIdAsync(result.ApprovalId);
                    logger.LogEntityCreated("AwardApproval", response!.ApprovalId);
                    return response!;
                }
                catch(Exception e)
                {
                    logger.LogOperationError("CreateAwardApproval", e, request.AwardId, request.TeacherId);
                    throw;
                }

            }
        }

        public async Task DeleteAwardApproval(int awardApprovalId)
        {
            using (logger.BeginOperationScope("DeleteAwardApproval", ("AwardApprovalID", awardApprovalId)))
            using (var timer = logger.TimeOperation("DeleteAwardApproval"))
            {
                var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(awardApprovalId);
                if (approval is null) throw new NotFoundException($"The given AwardApprovalId {awardApprovalId} was not found");
                try
                {
                    await uow.AwardApproval.DeleteAwardApprovalAsync(approval);
                    await uow.SaveChangeAsync();
                    logger.LogEntityDeleted("AwardApproval", awardApprovalId);
                }catch(Exception e)
                {
                    logger.LogOperationError("DeleteAwardApproval", e, awardApprovalId);
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

        public async Task UpdateAwardApproval(int awardApprovalId, UpdateAwardApprovalRequest request)
        {
            using (logger.BeginOperationScope("UpdateAwardApproval", ("AwardApprovalID", awardApprovalId)))
            using (var timer = logger.TimeOperation("UpdateAwardApproval"))
            {
                logger.LogInformation("Update method for Admin");
                var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(awardApprovalId);
                if (approval is null) throw new NotFoundException($"The given AwardApprovalId {awardApprovalId} was not found");

                var award = await GetAwardOrThrowAsync(approval.AwardId);
                ApplyDecisionIfProvided(approval, request.decision);

                approval.DecisionDate = DateTime.UtcNow;
                if (approval.DecisionDate > award.ExpiredDate) throw new BadRequestException("The period allocated for decision-making has ended");
                if (!string.IsNullOrWhiteSpace(request.Comment)) approval.Comment = request.Comment;

                await uow.AwardApproval.UpdateAwardApprovalAsync(approval);
                await uow.SaveChangeAsync();

                _ = await uow.AwardApproval.CountApprovedAwardApprovalsByAwardId(approval.AwardId);
                await uow.SaveChangeAsync();
                logger.LogEntityUpdated("AwardApproval", awardApprovalId);
            }
        }
        public async Task UpdateAwardApprovalForTeacher(int awardApprovalId,int teacherId, UpdateAwardApprovalRequest request)
        {
            using (logger.BeginOperationScope("UpdateAwardApproval", ("AwardApprovalID", awardApprovalId)))
            using (var timer = logger.TimeOperation("UpdateAwardApproval"))
            {
                logger.LogInformation("Update method for Teacher");
                var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(awardApprovalId);
                if (approval is null) throw new NotFoundException($"The given AwardApprovalId {awardApprovalId} was not found");
                try
                {
                    var award = await GetAwardOrThrowAsync(approval.AwardId);
                    if (approval.decision != null) throw new BadRequestException("You cannot change your decision");
                    if (approval.TeacherId != teacherId) throw new ForbiddenException("You can only update approvals assigned to you");

                    ApplyDecisionIfProvided(approval, request.decision);

                    approval.DecisionDate = DateTime.UtcNow;
                    if (approval.DecisionDate > award.ExpiredDate) throw new BadRequestException("The period allocated for decision-making has ended");
                    if (!string.IsNullOrWhiteSpace(request.Comment)) approval.Comment = request.Comment;

                    await uow.AwardApproval.UpdateAwardApprovalAsync(approval);
                    await uow.SaveChangeAsync();

                    if (await uow.AwardApproval.CheckConfirmedApprovalsByAwardId(award.AwardId))
                    {
                        int approvalsNum = await uow.AwardApproval.CountApprovedAwardApprovalsByAwardId(approval.AwardId);
                        UpdateAwardStatus(award, approvalsNum);
                    }
                    await uow.SaveChangeAsync();
                    logger.LogEntityUpdated("AwardApproval", awardApprovalId);
                }catch(Exception e)
                {
                    logger.LogOperationError("UpdateAwardApproval", e, teacherId, awardApprovalId);
                    throw;
                }
            }
        }

        private void UpdateAwardStatus(Award award, int approvalsNum)
        { 
            uow.Award.SetRowVersion(award, award.RowVersion);
            if (approvalsNum == award.RequireApproval)
            {
                logger.LogInformation("Award {AwardId} met required approvals, setting Approved", award.AwardId);
                award.status = Status.Approved;
            }
            else
            {
                logger.LogInformation("Award {AwardId} doesn't meet required approvals, setting Approved", award.AwardId);
                award.status = Status.Rejected;
            }
            logger.LogEntityUpdated("Award", award.AwardId);
        }
    
        private async Task ValidateCreateRequestAsync(CreateAwardApprovalRequest request)
        {
            if (await uow.AwardApproval.ExistsAsync(u => u.AwardId == request.AwardId && u.TeacherId == request.TeacherId))
            {
                throw new ConflictException("Award Id and Teacher Id is already existed");
            }

            if (!await uow.Award.ExistsAsync(u => u.AwardId == request.AwardId))
            {
                throw new NotFoundException($"The given AwardId {request.AwardId} was not found");
            }

            if (!await uow.User.ExistsAsync(u => u.UserId == request.TeacherId))
            {
                throw new NotFoundException($"The given TeacherId {request.TeacherId} was not found");
            }

            if (!await uow.User.IsTeacherAsync(request.TeacherId))
            {
                throw new BadRequestException($"The given Id {request.TeacherId} was not owned by a Teacher");
            }
        }

        private async Task<Award> GetAwardOrThrowAsync(int awardId)
        {
            return await uow.Award.GetAwardViaId(awardId)
                ?? throw new NotFoundException($"The given AwardId {awardId} was not found");
        }

        private static void ApplyDecisionIfProvided(AwardApproval approval, string? decision)
        {
            if (string.IsNullOrWhiteSpace(decision))
            {
                return;
            }

            switch (decision.Trim().ToLowerInvariant())
            {
                case "approve":
                    approval.decision = Decision.Approve;
                    break;
                case "reject":
                    approval.decision = Decision.Reject;
                    break;
                default:
                    throw new BadRequestException("Invalid input, it must be 'Approve' or 'Reject'");
            }
        }
    }

}
