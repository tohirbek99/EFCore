using System.ComponentModel.DataAnnotations;

namespace EFCore.Models
{
    public class Page
    {
        public int PageId { get; set; }
        [Required, MinLength(2,ErrorMessage ="Minimum length 2")]
        public string? Title { get; set; }
        public string? Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "Minimum length 4")]
        public string? Content { get; set; }
        public int Sorting { get; set; }
    }
}
