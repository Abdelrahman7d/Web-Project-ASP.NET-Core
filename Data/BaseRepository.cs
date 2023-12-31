using Data.Entity;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class BaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        private IQueryable<TEntity> _PrepareQuery(Expression<Func<TEntity, bool>>? filter, List<string>? includes = null)
        {

            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>().AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = GetAllInclude(query, includes);

            }

            return query;
        }

        private IQueryable<TEntity> GetAllInclude(IQueryable<TEntity> query, List<string> includesPaths)
        {
            return includesPaths.Where(s => !string.IsNullOrEmpty(s))
                .Aggregate(query, (current, path) => current.Include(path).AsNoTracking());
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _PrepareQuery(filter, includes).ToListAsync();
        }

        public async Task<TEntity?> GetEntityAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _PrepareQuery(filter, includes).FirstOrDefaultAsync();
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            _dbSet.Attach(entity); 
            _context.Entry(entity).State = EntityState.Added; 

            await _context.SaveChangesAsync();
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            var existingEntity = await _dbSet.FindAsync(entity.Id);

            if (existingEntity != null)
            {
                _context
                    .Entry(existingEntity)
                    .CurrentValues
                    .SetValues(entity);

                _context.Entry(existingEntity).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteEntityAsync(TEntity entity)
        {
            if (entity != null)
            {
                entity.IsDeleted = true;

                await UpdateEntityAsync(entity);
            }
        }

        public async Task<int> GetAllCountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _PrepareQuery(filter).CountAsync();
        }

    }
}