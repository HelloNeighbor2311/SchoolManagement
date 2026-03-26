using SchoolManagement.DTOs.Award;
using SchoolManagement.Models;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IAwardRepository: IGenericRepository<Award>
    {
        Task<List<AwardResponse>> GetAllAwardsAsync();
        Task<Award> CreateAwardAsync(Award award);
        Task<AwardResponse?> GetAwardResponseViaId(int awardId);
        Task CheckRequireApprovalsAsync(Award award, int approvedNums);
        Task<Award?> GetAwardViaId(int awardId);
        Task DeleteAwardAsync(Award award);
    }
}
