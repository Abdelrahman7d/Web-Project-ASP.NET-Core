using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Business.Query
{
    public interface IQueryBuilder<TQuery, TEntity>
    {
        public IQueryBuilder<TQuery, TEntity> WithFilter(Expression<Func<TEntity, bool>> filter);

        public IQueryBuilder<TQuery, TEntity> WithIncludes(List<string> includesPaths);
        TQuery Build();
    }
}
