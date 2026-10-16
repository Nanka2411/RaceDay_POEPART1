using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.DTOs
{
    public class CreateEventDto
    {
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

        [Range(typeof(decimal), "0.01", "1000")]
        public decimal Distance { get; set; }

        [Required]
        [RegularExpression("^(Run|Walk|Cycle)$",
            ErrorMessage = "EventType must be Run, Walk, or Cycle.")]
        public string EventType { get; set; } = "Run";
    }
}