using EmployeeLeaveManagementDAL.Data.Dbcontext;
using EmployeeLeaveManagementDAL.Data.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementDAL.Data.Repositories.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;
        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }
        public void Add(TEntity entity) => _dbSet.Add(entity);

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public void Update(TEntity entity) => _dbSet.Update(entity);

      

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _dbSet.ToListAsync(ct);
            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
                         => await _dbSet.FindAsync(id, ct);
         
    }
}
