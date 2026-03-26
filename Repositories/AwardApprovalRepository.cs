using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.DTOs.AwardApproval;
using SchoolManagement.Models;
using SchoolManagement.Repositories.Interfaces;

namespace SchoolManagement.Repositories
{
    public class AwardApprovalRepository(AppDbContext context, IMapper mapper) : GenericRepository<AwardApproval>(context), IAwardApprovalRepository
    {
        public async Task<AwardApproval> CreateApprovalAsync(AwardApproval approval)
        {
            await Context.AwardApprovals.AddAsync(approval);
            return approval;
        }

        public async Task DeleteAwardApprovalAsync(AwardApproval approval)
        {
            Context.Remove(approval);
        }

        public Task<List<AwardApprovalResponse>> GetAllApprovalsAsync()
        {
            return Context.AwardApprovals.ProjectTo<AwardApprovalResponse>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<AwardApprovalResponse?> GetAwardApprovalResponseViaIdAsync(int id)
        {
            return await Context.AwardApprovals.ProjectTo<AwardApprovalResponse>(mapper.ConfigurationProvider).FirstOrDefaultAsync(u => u.ApprovalId == id);
        }

        public async Task<AwardApproval?> GetAwardApprovalViaIdAsync(int id)
        {
            return await Context.AwardApprovals.FirstOrDefaultAsync(u => u.ApprovalId == id);
        }

        public async Task<List<AwardApprovalResponse>> GetListApprovalsViaTeacherIdAsync(int teacherId)
        {
            return await Context.AwardApprovals.Where(u => u.TeacherId == teacherId).ProjectTo<AwardApprovalResponse>(mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task UpdateAwardApprovalAsync(AwardApproval approval)
        {
            Context.Update(approval);
        }
    }
}
