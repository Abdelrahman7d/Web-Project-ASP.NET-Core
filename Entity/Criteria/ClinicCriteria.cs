using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Criteria
{
    public class ClinicCriteria: BaseCriteria
    {
        public int DepartmentId { get; set; } = -1;
    }
}
