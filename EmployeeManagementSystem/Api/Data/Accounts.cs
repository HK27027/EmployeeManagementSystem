using System.ComponentModel.DataAnnotations;

namespace Api.Data
{
    public class Accounts
    {
        [Key]
        public int AccountID { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public bool IsDeleted { get; set; } = false;

        public int? CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
