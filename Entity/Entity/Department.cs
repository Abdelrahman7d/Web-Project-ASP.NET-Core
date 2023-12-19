
using System.ComponentModel.DataAnnotations;

namespace Data.Entity
{
    public class Department : BaseEntity
    {
        [MaxLength(50)]
        [Required]
        public string DepartmentNameEn { get; set; }
        [MaxLength(50)]
        [Required]
        public string DepartmentNameAr { get; set; }
        public ICollection<Clinic>? Clinics { get; set; }
    }
}
