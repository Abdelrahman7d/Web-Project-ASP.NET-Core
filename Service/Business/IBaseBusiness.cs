using Entity;
using Entity.Criteria;
using Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public interface IBaseBusiness<TEntity, TDto>
    where TEntity : BaseEntity
    where TDto : BaseEntityDto
    {
        Task<List<TEntity>> GetAllAsync(BaseCriteria? criteria = null, List<string>? includes = null);
        Task<TEntity?> FindEntityByIdAsync(int id, List<string>? includes = null);
        Task<TEntity?> FindEntityAsync(BaseCriteria? criteria, List<string>? includes = null);
        Task AddEntityAsync(TEntity entity);
        Task UpdateEntityAsync(TEntity entity);
        Task DeletEntityAsync(TEntity entity);
        Task DeleteEntityByIdAsync(int id);
        Task<List<TDto>> GetAllDtoAsync(BaseCriteria? criteria = null, List<string>? includes = null);
        Task<TDto> GetEntityIdDtoAsync(int id, List<string>? includes = null);
        Task<int> GetAllCountAsync(BaseCriteria? criteria);
    }
}
