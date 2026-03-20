using System.Linq.Expressions;

namespace SchoolManagement.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
