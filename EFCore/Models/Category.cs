using System.ComponentModel.DataAnnotations;

namespace EFCore.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        [Required,MinLength(2,ErrorMessage ="Minimum Lemgth is 2")]
        [RegularExpression(@"^[a-zA-Z-]+$", ErrorMessage ="Only letters are allow")]
        public string Name { get; set; }
        public string?  Slug { get; set; }
        public int Sorting { get; set; }


    }
}
