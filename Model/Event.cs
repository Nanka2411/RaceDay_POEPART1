using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.Models
{
    public class Event
    {
        public int EventID { get; set; }

        [Required]
        [StringLength(55)]
        public string EventName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EventDescription { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(55)]
        public string EventLocation { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string RouteInformation { get; set; } = string.Empty;

        public int OrganiserID { get; set; }

        public decimal Distance { get; set; }

        [Required]
        [StringLength(20)]
        public string EventType { get; set; } = "Run";
    }
}
