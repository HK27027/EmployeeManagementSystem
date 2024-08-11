using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.Data
{
    public class Employees
    {
        [Key]
        public int EmployeeID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        public string Position { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentID { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;

        public virtual Departments Department { get; set; }
    }
}
