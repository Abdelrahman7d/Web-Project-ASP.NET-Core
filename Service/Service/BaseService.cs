using AutoMapper;
using Core.Exceptions;
using Core.Resources;
using Data.Entity;
using Entity.Criteria;
using Entity.DTO;
using Repository;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Service.Service
{
    public class BaseService<TEntity, TDto> where TEntity : BaseEntity where TDto : BaseEntityDto
    {
        protected readonly BaseRepository<TEntity> _baseRepository;
        protected readonly IMapper _mapper;
        protected readonly ResourceManagerService<ErrorMessages> _resourceManagerService;

        public BaseService(
            BaseRepository<TEntity> baseRepository,
            IMapper mapper,
            ResourceManagerService<ErrorMessages> resourceManagerService
            )
        {
            _baseRepository = baseRepository;
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

        virtual public async Task<List<TEntity>> GetAllAsync(List<string>? includes = null)
        {
            return await _baseRepository.GetAllAsync(_PrepareFilter(), includes);
        }

        public async Task<TEntity?> GetEntityByIdAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _baseRepository.GetEntityAsync(filter, includes);
        }

        public async Task<TEntity?> GetEntityByIdAsync(int id, List<string>? includes = null)
        {
            return await GetEntityAsync(_PrepareFilter(new BaseCriteria() { Id = id }), includes);
        }

        public async Task<TEntity?> GetEntityAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _baseRepository.GetEntityAsync(filter, includes);
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            await _baseRepository.AddEntityAsync(entity);
        }

        public async Task UpdateEntityAsync(TEntity entity)
        {
            await _baseRepository.UpdateEntityAsync(entity);
        }

        virtual public async Task DeleteEntityAsync(int id)
        {
            var entity = await GetEntityByIdAsync(_PrepareFilter(new BaseCriteria() { Id = id }));

            if (entity != null)
            {
                await _baseRepository.DeleteEntityAsync(entity);
            }
            else
            {
                throw new BaseException(_resourceManagerService.GetString("NOT_FOUND_ENTITY"),ErrorCode.BusinessError);
            }

        }

        public async Task<List<TDto>> GetAllDtoAsync(List<string>? includes = null)
        {
            var entities = await GetAllAsync(includes);


            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> GetEntityIdDtoAsync(int id,List<string>? includes = null)
        {
            var entities = await GetEntityByIdAsync(id,includes);


            return _mapper.Map<TDto>(entities);
        }



    }
}