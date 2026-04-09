using SchoolManagement.DTOs.Award;

namespace SchoolManagement.Services.Interfaces
{
    public interface IAwardService
    {
        Task<List<AwardResponse>> GetAllAwards();
        Task<AwardResponse> CreateAward(CreateAwardRequest request);
        Task<AwardResponse> UpdateAward(int awardId, UpdateAwardRequest request);
        Task<AwardResponse> UpdateAwardStatus(int awardId, UpdateAwardRequest request);
        Task DeleteAward(int awardId);
    }
}
