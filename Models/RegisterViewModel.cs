
using System.ComponentModel.DataAnnotations;

namespace RaceDayMVC.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(55)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(55)]
        public string LastName { get; set; } = "";

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = "";

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; } = "";

        [Required]
        public string UserRole { get; set; } = "Participant";

        [Range(1, 120)]
        public int? ParticipantAge { get; set; }

        [MaxLength(100)]
        public string? OrganiserName { get; set; }

        [MaxLength(100)]
        public string? OrganisationName { get; set; }
    }
}
