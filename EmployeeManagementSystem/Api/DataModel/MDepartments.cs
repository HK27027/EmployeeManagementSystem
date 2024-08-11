using System.ComponentModel.DataAnnotations;

namespace Api.DataModel
{
    public class MDepartments
    {
        public class Response
        {

            [Key]
            public int DepartmentID { get; set; }

            [Required]
            [StringLength(50)]
            public string DepartmentName { get; set; }

            public bool IsDeleted { get; set; } = false;
            public List<MEmployees.Response> Employees { get; set; }

            public int? CreatedBy { get; set; }
            public int? EmpolyeCount { get; set; }

            public DateTime CreatedTime { get; set; } = DateTime.Now;
        }


        public class Form
        {

           
            public int? AccountID { get; set; }
            public int? DepartmentID { get; set; }

            public string? DepartmentName { get; set; }
        }
    }
}
