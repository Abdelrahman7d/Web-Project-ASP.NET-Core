using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Criteria
{
    public class BaseCriteria
    {
        public int Id { get; set; } = -1;
        public bool IsDeleted { get; set; } = false;
    }
}
