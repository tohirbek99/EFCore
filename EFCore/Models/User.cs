using System.ComponentModel.DataAnnotations;

namespace EFCore.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }
    }
}
