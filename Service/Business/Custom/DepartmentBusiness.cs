using AutoMapper;
using Data.Query;
using Core.Core;
using Core.Exceptions;
using Entity;
using Entity.Criteria;
using Entity.DTO;
using Data.Repository;

namespace Business.Custom
{
    public class DepartmentBusiness : BaseBusiness<Department, DepartmentDto>
    {
        private ClinicBusiness _clinicService;
        public DepartmentBusiness(
            IRepositoryBase<IQueryable<Department>, Department> repository,
            IQueryBuilder<IQueryable<Department>, Department> queryBuilder,
            IMapper mapper,
            IResourceManagerService resourceManagerService,
            ClinicBusiness clinicService
            )
            : base(repository, queryBuilder, mapper, resourceManagerService)
        {
            _clinicService = clinicService;
        }

        override public async Task DeleteEntityByIdAsync(int id)
        {
            BaseCriteria criteria = new BaseCriteria()
            {
                Id = id
            };

            var entity = await FindEntityAsync(criteria);

            if (entity != null)
            {
                int clinicsCount = await _clinicService.GetAllClincsCountByDepartmentIdAsync(id);
                if (clinicsCount > 0)
                {
                    throw new BaseException(_resourceManagerService.GetString("DEPARTMENT_HAS_CLINICS"), ErrorCode.BusinessError);
                }
                else
                {
                    await _baseRepository.DeleteEntityAsync(entity);
                }
            }
            else
            {
                throw new BaseException(_resourceManagerService.GetString("DEPARTMENT_NOT_FOUND"), ErrorCode.BusinessError);
            }
        }
    }

}