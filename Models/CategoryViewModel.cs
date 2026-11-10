
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required]
        [MaxLength(80)]
        public string CategoryName { get; set; } = "";

        public string? CategoryDescription { get; set; }
    }
}
