using SchoolManagement.DTOs.AwardApproval;

namespace SchoolManagement.Services.Interfaces
{
    public interface IAwardApprovalService
    {
        Task<AwardApprovalResponse> CreateAwardApproval(CreateAwardApprovalRequest request);
        Task<List<AwardApprovalResponse>> GetAllAwardApprovals();
        Task<List<AwardApprovalResponse>> GetAwardApprovalsViaTeacherId(int teacherId);
        Task UpdateAwardApproval(int awardApprovalId, UpdateAwardApprovalRequest request);
        Task UpdateAwardApprovalForTeacher(int awardApprovalId, int teacherId, UpdateAwardApprovalRequest request);
        Task DeleteAwardApproval(int awardApprovalId);
    }
}
