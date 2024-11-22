using Project.Data.Entities;
using Project.Comman.Idenitity;

namespace Project.Data.IGenericRepository_IUOW
{
    public interface IUnitOfWork : IDisposable
    {
     
        public IGeneralRepository<ApplicationUser> Users { get; }
        public IGeneralRepository<Customer> Customers { get; }
        
        Task<bool> SaveAsync();
    }
}
