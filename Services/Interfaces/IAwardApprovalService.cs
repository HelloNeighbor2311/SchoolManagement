using SchoolManagement.DTOs.AwardApproval;

namespace SchoolManagement.Services.Interfaces
{
    public interface IAwardApprovalService
    {
        Task<AwardApprovalResponse> CreateAwardApproval(CreateAwardApprovalRequest request);
        Task<List<AwardApprovalResponse>> GetAllAwardApprovals();
        Task<List<AwardApprovalResponse>> GetAwardApprovalsViaTeacherId(int teacherId);
        Task UpdateAwardApproval(int id, UpdateAwardApprovalRequest request);
        Task DeleteAwardApproval(int id);
    }
}
