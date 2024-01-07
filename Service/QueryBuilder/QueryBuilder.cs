using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Data;
using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Business.Query
{
    public class QueryBuilder<TQuery, TEntity> : IQueryBuilder<TQuery,TEntity> 
        where TQuery : IQueryable<TEntity>
        where TEntity : BaseEntity
    {
        private IQueryable<TEntity> _query;

        public QueryBuilder(AppDbContext context)
        {
            _query = context
                .Set<TEntity>()
                .AsNoTracking();
        }        

        public IQueryBuilder<TQuery,TEntity> WithFilter(Expression<Func<TEntity, bool>> filter)
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

        public TQuery Build() => (TQuery) _query;
    }
}
