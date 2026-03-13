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
        public ICourseRepository Courses { get; }
        public UnitOfWork(AppDbContext context, IUserRepository userRepository, ICourseRepository courseRepository)
        {
            this.context = context;
            Users = userRepository;
            Courses = courseRepository;
        }
        public async Task<int> SaveChangeAsync()
        {
            return await context.SaveChangesAsync();
        }
        public void Dispose() => context.Dispose();
    }
}
