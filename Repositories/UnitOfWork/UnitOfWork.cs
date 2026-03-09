using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext context;
        public IUserRepository Users { get; }
        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
            Users = new UserRepository(context);
        }
        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose() => context.Dispose();
    }
}
