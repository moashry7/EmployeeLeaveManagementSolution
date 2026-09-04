using EmployeeLeaveManagementDAL.Data.Dbcontext;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;


namespace EmployeeLeaveManagementDAL.Data.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName, out object? repository))
                return (IRepository<TEntity>)repository;
            else
            {
                var repo = new Repository<TEntity>(_dbContext);
                _repositories[typeName] = repo;
                return repo;
            }

        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _dbContext.SaveChangesAsync(ct);

    }
}
