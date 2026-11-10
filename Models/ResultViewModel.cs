using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }

        [Required]
        public int EntryID { get; set; }

        [Required]
        [RegularExpression(
            @"^\d{1,2}:\d{2}:\d{2}$",
            ErrorMessage = "Enter the time in HH:mm:ss format.")]
        public string ResultTime { get; set; } = "";

        [Required]
        [Range(1, int.MaxValue)]
        public int Position { get; set; }

        public string ResultStatus { get; set; } = "Completed";
    }
}