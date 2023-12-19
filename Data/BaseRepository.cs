using Data.Entity;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;

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

            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();

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
        public IQueryable<TEntity> GetAllInclude(IQueryable<TEntity> query, List<string> includesPaths)
        {
            return includesPaths.Where(s => !string.IsNullOrEmpty(s))
                .Aggregate(query, (current, path) => current.Include(path).AsNoTracking());
        }

        public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _PrepareQuery(filter, includes).ToListAsync();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return _PrepareQuery(filter, includes).ToList();
        }

        public async Task<TEntity?> GetEntityByIdAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _PrepareQuery(filter, includes).FirstOrDefaultAsync();
        }

        public TEntity? GetEntityById(Expression<Func<TEntity, bool>> filter, List<string>? includes)
        {
            return _PrepareQuery(filter, includes).FirstOrDefault();
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void AddEntity(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            var existingEntity = await _dbSet.FindAsync(entity.Id);

            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
            
        }

        public void UpdateEntity(TEntity entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public async Task DeleteEntityAsync(TEntity entity)
        {
            if (entity != null)
            {
                entity.IsDeleted = true;

                await UpdateEntityAsync(entity);
            }
        }

        public void DeleteEntity(int id)
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
        }

        public void DeleteEntity(TEntity entity)
        {
            if (entity != null)
            {
                entity.IsDeleted = true;
                UpdateEntity(entity);  
            }
        }

        public int GetAllCount(Expression<Func<TEntity, bool>> filter)
        {
            return _PrepareQuery(filter).Count();
        }

    }
}