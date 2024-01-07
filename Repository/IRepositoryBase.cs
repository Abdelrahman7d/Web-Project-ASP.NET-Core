using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Repository
{
    public interface IRepositoryBase<TQuery, TEntity>
    {
        public Task<List<TEntity>> GetAllAsync(TQuery query);

        public Task<TEntity?> FindEntityAsync(TQuery query);

        public Task AddEntityAsync(TEntity entity);

        public Task UpdateEntityAsync(TEntity entity);

        public Task DeleteEntityAsync(TEntity entity);

        public Task<int> GetAllCountAsync(TQuery query);
    }
}
