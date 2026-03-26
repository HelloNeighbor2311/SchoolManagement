using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IAwardApprovalRepository: IGenericRepository<AwardApproval>
    {
        Task<List<AwardApprovalResponse>> GetAllApprovalsAsync();
        Task<List<AwardApprovalResponse>> GetListApprovalsViaTeacherIdAsync(int teacherId);
        Task<AwardApproval> CreateApprovalAsync(AwardApproval approval);
        Task<AwardApprovalResponse?> GetAwardApprovalResponseViaIdAsync(int id);
        Task<AwardApproval?> GetAwardApprovalViaIdAsync(int id);
        Task UpdateAwardApprovalAsync(AwardApproval approval);
        Task DeleteAwardApprovalAsync(AwardApproval approval);
    }
}
