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
        public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public void Delete(TEntity entity) => _dbSet.Remove(entity);

        public void Edit(TEntity entity) => _dbSet.Update(entity);

        public async Task<bool> ExistsAsync(int id) => await _dbSet.FindAsync(id) != null;

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var entities = await _dbSet.ToListAsync();
            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }
    }
}
