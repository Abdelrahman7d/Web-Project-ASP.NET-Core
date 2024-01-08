using System.Linq.Expressions;
using Data;
using Entity;
using Microsoft.EntityFrameworkCore;

namespace Data.Query
{
    public class QueryBuilder<TQuery, TEntity> : IQueryBuilder<TQuery, TEntity>
        where TQuery : IQueryable<TEntity>
        where TEntity : BaseEntity
    {
        private IQueryable<TEntity> _query;

        AppDbContext _dbContext;
        public QueryBuilder(AppDbContext context)
        {
            _dbContext = context;
            _query = _DefaultQuery();
        }

        private IQueryable<TEntity> _DefaultQuery()
        {
            return _dbContext
                .Set<TEntity>()
                .AsNoTracking();
        }
        public IQueryBuilder<TQuery, TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
        {

            if (filter != null)
            {
                _query = _query.Where(filter);
            }
            return this;
        }

        public IQueryBuilder<TQuery, TEntity> WithIncludes(List<string> includesPaths)
        {
            if (includesPaths != null)
            {
                _query = includesPaths.Where(s => !string.IsNullOrEmpty(s))
                                      .Aggregate(_query, (current, path) => current.Include(path));
            }
            return this;
        }

        public TQuery Build() 
        {
            IQueryable<TEntity> queryToBuild = _query;

            //By this step, We make sure to reset the _query value to the default value in the dbcontext
            //to avoid any repetition between queries.
            _query = _DefaultQuery();

            return (TQuery) queryToBuild;
            
        }
    }
}
