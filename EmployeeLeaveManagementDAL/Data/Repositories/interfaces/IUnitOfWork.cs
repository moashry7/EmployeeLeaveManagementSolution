using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeLeaveManagementDAL.Data.Repositories.interfaces
{
    public interface IUnitOfWork
    {

         IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
            Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
