using AutoMapper;
using Data.Query;
using Core.Core;
using Core.Exceptions;
using Entity;
using Entity.Criteria;
using Entity.DTO;
using Data.Repository;
using System.Linq.Expressions;

namespace Business
{
    public class BaseBusiness<TEntity, TDto> 
        where TEntity : BaseEntity 
        where TDto : BaseEntityDto
    {
        protected readonly IRepositoryBase<IQueryable<TEntity>,TEntity> _baseRepository;
        protected readonly IQueryBuilder<IQueryable<TEntity>, TEntity> _queryBuilder;
        protected readonly IMapper _mapper;
        protected readonly IResourceManagerService _resourceManagerService;

        public BaseBusiness(
            IRepositoryBase<IQueryable<TEntity>,TEntity> baseRepository,
            IQueryBuilder<IQueryable<TEntity>,TEntity> queryBuilder,
            IMapper mapper,
            IResourceManagerService resourceManagerService
            )
        {
            _baseRepository = baseRepository;
            _queryBuilder = queryBuilder;
            _mapper = mapper;
            _resourceManagerService = resourceManagerService;
        }

        protected Expression<Func<TEntity, bool>> _PrepareFilter(BaseCriteria? criteria = null)
        {
            Expression<Func<TEntity, bool>> filter;

            if (criteria != null)
            {
                filter = entity => criteria.IsDeleted == entity.IsDeleted;

                if (criteria.Id > -1)
                {
                    filter = filter.And(entity => criteria.Id == entity.Id);
                }
            }
            else
            {
                filter = entity => !entity.IsDeleted;
            }

            return filter;
        }

        protected IQueryable<TEntity> _BuildQeury(Expression<Func<TEntity, bool>>? filter, List<string>? includes = null)
        {
            return _queryBuilder
                .WithFilter(filter)
                .WithIncludes(includes)
                .Build();
        }

        virtual public async Task<List<TEntity>> GetAllAsync(BaseCriteria? criteria = null, List<string>? includes = null)
        {
            var query = _BuildQeury(_PrepareFilter(criteria), includes);
            return await _baseRepository.GetAllAsync(query);
        }

        public async Task<TEntity?> FindEntityByIdAsync(int id, List<string>? includes = null)
        {
            return await FindEntityAsync(new BaseCriteria() { Id = id }, includes);
        }

        public async Task<TEntity?> FindEntityAsync(BaseCriteria? criteria, List<string>? includes = null)
        {
            var query = _BuildQeury(_PrepareFilter(criteria), includes);
            return await _baseRepository.FindEntityAsync(query);
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            await _baseRepository.AddEntityAsync(entity);
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            await _baseRepository.UpdateEntityAsync(entity);
        }

        public async Task DeletEntityAsync(TEntity entity)
        {
            await DeleteEntityByIdAsync(entity.Id);
        }

        virtual public async Task DeleteEntityByIdAsync(int id)
        {
            var entity = await FindEntityByIdAsync(id);

            if (entity != null)
            {
                await _baseRepository.DeleteEntityAsync(entity);
            }
            else
            {
                throw new BaseException(_resourceManagerService.GetString("NOT_FOUND_ENTITY"),ErrorCode.BusinessError);
            }

        }

        public async Task<List<TDto>> GetAllDtoAsync(BaseCriteria? criteria = null, List<string>? includes = null)
        {
            var entities = await GetAllAsync(criteria,includes);

            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> FindEntityIdDtoAsync(int id,List<string>? includes = null)
        {
            var entities = await FindEntityByIdAsync(id,includes);
            return _mapper.Map<TDto>(entities);
        }

        public async Task<int> GetAllCountAsync(BaseCriteria? criteria)
        {
            var query = _BuildQeury(_PrepareFilter(criteria));

            return await _baseRepository.GetAllCountAsync(query);
        }
    }
}