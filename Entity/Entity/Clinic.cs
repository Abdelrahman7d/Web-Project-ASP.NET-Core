using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class Clinic: BaseEntity
    {
        [MaxLength(50)]
        [Required]
        public string ClinicNameAr { get; set; }
        [MaxLength(50)]
        [Required]
        public string ClinicNameEn { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(10)]
        public string PhoneNumber { get; set; }
        [DefaultValue(false)]
        public bool Status { get; set; } = false;
    }
}
