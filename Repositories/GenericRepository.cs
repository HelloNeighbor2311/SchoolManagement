using Microsoft.EntityFrameworkCore;
using SchoolManagement.Datas;
using SchoolManagement.Repositories.Interfaces;
using System.Linq.Expressions;

namespace SchoolManagement.Repositories
{
    public class GenericRepository<T>: IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext Context;
        protected readonly DbSet<T> DbSet;
        public GenericRepository(AppDbContext _context)
        {
            Context = _context;
            DbSet = _context.Set<T>();
        }
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate) => await DbSet.AnyAsync(predicate);
    }
}
