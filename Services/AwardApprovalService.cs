using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Exceptions;
using SchoolManagement.Models;
using SchoolManagement.Repositories.UnitOfWork;
using SchoolManagement.Services.Interfaces;

namespace SchoolManagement.Services
{
    public class AwardApprovalService(IUnitOfWork uow, IMapper mapper) : IAwardApprovalService
    {
        public async Task<AwardApprovalResponse> CreateAwardApproval(CreateAwardApprovalRequest request)
        {
            if (await uow.AwardApproval.ExistsAsync(u => u.AwardId == request.AwardId && u.TeacherId == request.TeacherId)) throw new ConflictException("Award Id and Teacher Id is already existed");
            if (!await uow.Award.ExistsAsync(u=>u.AwardId == request.AwardId)) throw new BadRequestException($"The given AwardId {request.AwardId} was not found");
            if (!await uow.User.ExistsAsync(u => u.UserId == request.TeacherId)) throw new NotFoundException($"The given TeacherId {request.TeacherId} was not found");

            if (!await uow.User.IsTeacherAsync(request.TeacherId)) throw new BadRequestException($"The given Id {request.TeacherId} was not owned by a Teacher");
            var approval = mapper.Map<AwardApproval>(request);
            var result = await uow.AwardApproval.CreateApprovalAsync(approval);
            await uow.SaveChangeAsync();
            var response = await uow.AwardApproval.GetAwardApprovalResponseViaIdAsync(result.ApprovalId);
            return response!;
        }

        public async Task DeleteAwardApproval(int id)
        {
            var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(id);
            if (approval is null) throw new NotFoundException($"The given AwardApprovalId {id} was not found");
            await uow.AwardApproval.DeleteAwardApprovalAsync(approval);
            await uow.SaveChangeAsync();
        }

        public async Task<List<AwardApprovalResponse>> GetAllAwardApprovals()
        {
            return await uow.AwardApproval.GetAllApprovalsAsync();
        }

        public async Task<List<AwardApprovalResponse>> GetAwardApprovalsViaTeacherId(int teacherId)
        {
            return await uow.AwardApproval.GetListApprovalsViaTeacherIdAsync(teacherId);
        }

        public async Task UpdateAwardApproval(int id, UpdateAwardApprovalRequest request)
        {
            var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(id);
            if (approval is null) throw new NotFoundException($"The given AwardApprovalId {id} was not found");
            if (request.decision != null)
            {
                switch (request.decision.ToLower())
                {
                    case "Approve":
                        approval.decision = Decision.Approve;
                        break;
                    case "Reject":
                        approval.decision = Decision.Reject;
                        break;
                    default:
                        throw new BadRequestException("Invalid input");
                }
            }
        }
        public async Task UpdateAwardApprovalForTeacher(int id,int teacherId, UpdateAwardApprovalRequest request)
        {
            var approval = await uow.AwardApproval.GetAwardApprovalViaIdAsync(id);
            if (approval is null) throw new NotFoundException($"The given AwardApprovalId {id} was not found");
            var award = await uow.Award.GetAwardViaId(approval.AwardId);
            if (approval.TeacherId != teacherId) throw new ForbiddenException("You can only update approvals assigned to you");
            if (approval.decision != null) throw new BadRequestException("Cannot change decision has already been designed");

            if (request.decision != null)
            {
                switch (request.decision.ToLower())
                {
                    case "Approve":
                        approval.decision = Decision.Approve;
                        break;
                    case "Reject":
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
        }
    }
}
