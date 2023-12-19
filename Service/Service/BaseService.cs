using AutoMapper;
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

        public BaseService(BaseRepository<TEntity> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }

        protected Expression<Func<TEntity, bool>> _PrepareFilter(BaseCriteria? criteria = null)
        {
            Expression<Func<TEntity, bool>> filter;

            if (criteria != null)
            {
                filter = entity => criteria.IsDeleted == entity.IsDeleted;

                if (criteria.Id > -1)
                {
                    filter = filter.And(entity => criteria.Id == entity.Id );
                }
            }
            else
            {
                filter = entity => !entity.IsDeleted;
            }

            return filter;
        }

        virtual public async Task<List<TEntity>> GetAllEntitiesAsync(List<string>? includes = null)
        {
            return await _baseRepository.GetAllAsync(_PrepareFilter(), includes);
        }

        virtual public List<TEntity> GetAllEntities(List<string>? includes = null)
        {
            return _baseRepository.GetAll(_PrepareFilter(), includes).ToList();
        }

        public async Task<TEntity?> GetEntityByIdAsync(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return await _baseRepository.GetEntityByIdAsync(filter, includes);
        }
        public async Task<TEntity?> GetEntityByIdAsync(int id, List<string>? includes = null)
        {
            return await GetEntityByIdAsync(_PrepareFilter(new BaseCriteria() { Id = id }), includes);
        }

        public TEntity? GetEntityById(Expression<Func<TEntity, bool>> filter, List<string>? includes = null)
        {
            return _baseRepository.GetEntityById(filter, includes);
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
            var entity = GetEntityById(_PrepareFilter(new BaseCriteria() { Id = id }));

            if (entity != null)
            {
                await _baseRepository.DeleteEntityAsync(entity);
            }

        }

        public async Task<List<TDto>> GetAllDtoAsync(List<string>? includes = null)
        {
            var entities = await GetAllEntitiesAsync(includes);


            return _mapper.Map<List<TDto>>(entities);
        }

        public async Task<TDto> GetEntityIdDtoAsync(int id,List<string>? includes = null)
        {
            var entities = await GetEntityByIdAsync(id,includes);


            return _mapper.Map<TDto>(entities);
        }



    }
}