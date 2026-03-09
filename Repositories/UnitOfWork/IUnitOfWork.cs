using SchoolManagement.Datas;
using SchoolManagement.Models;
using SchoolManagement.Repositories;

namespace SchoolManagement.Repositories.UnitOfWork
{
    public interface IUnitOfWork: IDisposable
    {
        IUserRepository Users { get; }
        Task<int> SaveChangeAsync();
    }
}
