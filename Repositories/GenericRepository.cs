
using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;

namespace SchoolManagement.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext context;
        protected readonly DbSet<T> dbSet;
        public GenericRepository(AppDbContext _context)
        {
            context = _context;
            dbSet = _context.Set<T>();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }
    }
}
