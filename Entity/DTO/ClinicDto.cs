using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTO
{
    public class ClinicDto :BaseEntityDto
    {
        public string ClinicNameAr { get; set; }
        public string ClinicNameEn { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public bool Status { get; set; }
    }

}
