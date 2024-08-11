using System.ComponentModel.DataAnnotations;

namespace Api.Data
{
    public class Departments
    {

        [Key]
        public int DepartmentID { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
