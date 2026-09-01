namespace EmployeeLeaveManagementDAL.Data.Repositories.interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
        void Edit(TEntity entity);
        void Delete(TEntity entity);
        Task<bool> ExistsAsync(int id);

    }
}
