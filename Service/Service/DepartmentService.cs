using AutoMapper;
using Data.Entity;
using Entity.Criteria;
using Entity.DTO;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Linq.Expressions;

namespace Service.Service
{
    public class DepartmentService : BaseService<Department, DepartmentDto>
    {
        private ClinicService _clinicService;
        public DepartmentService(
            BaseRepository<Department> repository,
            IMapper mapper,
            ClinicService clinicService
            )
            : base(repository, mapper)
        {
            _clinicService = clinicService;
        }

        override public async Task DeleteEntityAsync(int id)
        {
            BaseCriteria criteria = new BaseCriteria()
            {
                Id = id
            };

            Expression<Func<Department, bool>> filter = _PrepareFilter(criteria);

            var entity = await _baseRepository.GetEntityByIdAsync(filter);

            if (entity != null)
            {
                int clinicsCount = _clinicService.GetAllClincsCountByDepartmentId(id);
                if (clinicsCount > 0)
                {
                    throw new Exception("This department has clinics!");
                }
                else
                {
                    await _baseRepository.DeleteEntityAsync(entity);
                }
            }
            else { 
                throw new Exception("This department is not found!"); 
            }
        }
    }

}