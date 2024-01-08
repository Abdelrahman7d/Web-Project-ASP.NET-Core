using AutoMapper;
using Entity;
using Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ClinicDto, Clinic>().ReverseMap();
            CreateMap<Department, DepartmentDto>().PreserveReferences();
        }
    }

}
