using System.Linq.Expressions;


namespace Data.Query
{
    public interface IQueryBuilder<TQuery, TEntity>
    {
        public IQueryBuilder<TQuery, TEntity> WithFilter(Expression<Func<TEntity, bool>> filter);

        public IQueryBuilder<TQuery, TEntity> WithIncludes(List<string> includesPaths);
        TQuery Build();
    }
}
