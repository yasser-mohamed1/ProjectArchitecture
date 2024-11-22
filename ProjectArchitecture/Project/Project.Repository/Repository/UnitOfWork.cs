using Project.Data.Entities;
using Project.Data.IGenericRepository_IUOW;
using Project.EntityFramework.DataBaseContext;
using Project.Comman.Idenitity;

namespace Project.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
     
        public IGeneralRepository<ApplicationUser> Users { get; private set; }
        public IGeneralRepository<Customer> Customers { get; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
          
            Users= new GeneralRepository<ApplicationUser>(_context);
            Customers= new GeneralRepository<Customer>(_context);
        }
        public async Task<bool> SaveAsync()
        {
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
