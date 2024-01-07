using Data.Entity;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repository
{
    public class BaseRepository<TQuery,TEntity> : IRepositoryBase<TQuery,TEntity>
        where TQuery : IQueryable<TEntity>
        where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected DbSet<TEntity> _dbSet;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }


        public async Task<List<TEntity>> GetAllAsync(TQuery query)
        {
            return await query.ToListAsync();
        }

        public async Task<TEntity?> FindEntityAsync(TQuery query)
        {
            return await query.FirstOrDefaultAsync();
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

        public async Task<int> GetAllCountAsync(TQuery query)
        {
            return await query.CountAsync();
        }

    }
}