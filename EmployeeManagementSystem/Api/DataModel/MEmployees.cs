using Api.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api.DataModel
{
    public class MEmployees
    {

        public class Response
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

            [ForeignKey("Department")]
            public int? DepartmentID { get; set; }

            public virtual Departments Department { get; set; }

            public bool IsDeleted { get; set; } = false;

            public int? CreatedBy { get; set; }

            public DateTime CreatedTime { get; set; } = DateTime.Now;

            public string Position { get; set; }
            public int? AccountID { get; set; }
        }

        public class Form
        {


            public int? AccountID { get; set; }
            public int? DepartmentID { get; set; }
            public int? EmployeeID { get; set; }
            public string? Position { get; set; }
            public string? Email { get; set; }
            public string? Name { get; set; }

        }
    }
}
