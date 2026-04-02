using SchoolManagement.DTOs.Award;

namespace SchoolManagement.Services.Interfaces
{
    public interface IAwardService
    {
        Task<List<AwardResponse>> GetAllAwards();
        Task<AwardResponse> CreateAward(CreateAwardRequest request);
        Task<AwardResponse> UpdateAward(int id, UpdateAwardRequest request);
        Task DeleteAward(int id);
    }
}
