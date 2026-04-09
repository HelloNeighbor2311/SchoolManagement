using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IAwardApprovalRepository: IGenericRepository<AwardApproval>
    {
        Task<List<AwardApprovalResponse>> GetAllApprovalsAsync();
        Task<List<AwardApprovalResponse>> GetListApprovalsViaTeacherIdAsync(int teacherId);
        Task<AwardApproval> CreateApprovalAsync(AwardApproval approval);
        Task<AwardApprovalResponse?> GetAwardApprovalResponseViaIdAsync(int awardApprovalId);
        Task<int> CountApprovedAwardApprovalsByAwardId(int awardId);
        Task<AwardApproval?> GetAwardApprovalViaIdAsync(int awardApprovalId);
        Task UpdateAwardApprovalAsync(AwardApproval approval);
        Task DeleteAwardApprovalAsync(AwardApproval approval);
    }
}
