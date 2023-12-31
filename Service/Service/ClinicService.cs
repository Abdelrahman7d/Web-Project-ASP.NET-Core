using AutoMapper;
using Core.Exceptions;
using Core.Resources;
using Data.Entity;
using Entity.Criteria;
using Entity.DTO;
using Repository;
using System.Linq.Expressions;
namespace Service.Service
{
    public class ClinicService : BaseService<Clinic, ClinicDto>
    {
        public ClinicService(
            BaseRepository<Clinic> repository,
            IMapper mapper,
            ResourceManagerService<ErrorMessages> resourceManagerService
            ) : base(repository, mapper, resourceManagerService)
        {
        }

        protected Expression<Func<Clinic, bool>> _PrepareClinicFilter(ClinicCriteria? criteria = null)
        {

            Expression<Func<Clinic, bool>> filter = _PrepareFilter(criteria);


            if(criteria != null)
            {
                if (criteria.DepartmentId > -1)
                {
                    filter = filter.And(entity => entity.DepartmentId == criteria.DepartmentId);
                }
            }

            return filter;
        }

        public async Task<int> GetAllClincsCountByDepartmentIdAsync(int departmentId) 
        {
            ClinicCriteria criteria = new ClinicCriteria() { DepartmentId = departmentId };

            int count = await _baseRepository.GetAllCountAsync(_PrepareClinicFilter(criteria));
            return count;
        }
    }

}
