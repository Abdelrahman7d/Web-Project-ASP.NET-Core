using AutoMapper;
using Business.Query;
using Core.Core;
using Core.Exceptions;
using Core.Resources;
using Data.Entity;
using Entity.Criteria;
using Entity.DTO;
using Repository;
using System.Linq.Expressions;

namespace Business.Custom
{
    public class ClinicBusiness : BaseBusiness<Clinic, ClinicDto>
    {
        public ClinicBusiness(
            IRepositoryBase<IQueryable<Clinic>, Clinic> repository,
            IQueryBuilder<IQueryable<Clinic>, Clinic> queryBuilder,
            IMapper mapper,
            IResourceManagerService resourceManagerService
            ) : base(repository, queryBuilder, mapper, resourceManagerService)
        {
        }

        protected Expression<Func<Clinic, bool>> _PrepareClinicFilter(ClinicCriteria? criteria = null)
        {

            Expression<Func<Clinic, bool>> filter = _PrepareFilter(criteria);


            if (criteria != null)
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
            var query = _BuildQeury(_PrepareClinicFilter(criteria));

            int count = await _baseRepository.GetAllCountAsync(query);

            return count;
        }
    }

}
