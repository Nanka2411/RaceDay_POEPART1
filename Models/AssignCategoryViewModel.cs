using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class AssignCategoryViewModel
    {
        [Required]
        public int CategoryID { get; set; }

        [Required]
        public int EventID { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
            = new List<CategoryViewModel>();

        public List<EventViewModel> Events { get; set; }
            = new List<EventViewModel>();
    }
}