
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        [Required]
        [StringLength(55)]
        public string EventName { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string EventDescription { get; set; } = "";

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(55)]
        public string EventLocation { get; set; } = "";

        [Required]
        [StringLength(200)]
        public string RouteInformation { get; set; } = "";

        [Range(typeof(decimal), "0.01", "1000")]
        public decimal Distance { get; set; }

        [Required]
        [RegularExpression("^(Run|Walk|Cycle)$")]
        public string EventType { get; set; } = "Run";
    }
}
