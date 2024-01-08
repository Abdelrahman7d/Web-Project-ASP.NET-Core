namespace Data.Repository
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
