using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entity;
using Microsoft.EntityFrameworkCore;


namespace Repository
{
    public class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        public IQueryable<TEntity> GetAllInclude(IQueryable<TEntity> query, List<string> includespaths)
        {
            return includespaths.Where(s => !string.IsNullOrEmpty(s)).Aggregate
              (query, (current, path) => current.Include(path).AsNoTracking());
        }
    }
}
