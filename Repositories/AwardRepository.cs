using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.Award;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class AwardRepository(AppDbContext context, IMapper mapper) : GenericRepository<Award>(context), IAwardRepository
    {
        public async Task<Award> CreateAwardAsync(Award award)
        {
            await Context.Awards.AddAsync(award);
            return award;
        }

        public async Task DeleteAwardAsync(Award award)
        {
            Context.Awards.Remove(award);
        }

        public async Task<List<AwardResponse>> GetAllAwardsAsync()
        {
            var result = await Context.Awards.ProjectTo<AwardResponse>(mapper.ConfigurationProvider).ToListAsync();
            return result;
        }

        public async Task<AwardResponse?> GetAwardResponseViaId(int awardId)
        {
            var result = await Context.Awards.ProjectTo<AwardResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.AwardId == awardId);
            return result;
        }

        public async Task<Award?> GetAwardViaId(int awardId)
        {
            var result = await Context.Awards.FindAsync(awardId);
            return result;
        }

        public void SetRowVersion(Award award, byte[] rowVersion)
        {
            Context.Entry(award).Property(a => a.RowVersion).OriginalValue = rowVersion;
        }
    }
}
