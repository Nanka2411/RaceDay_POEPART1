
using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.DTOs;

public class CreateAccountDto
{
    [Required, MaxLength(55)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(55)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(8)]
    public string Password { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string UserRole { get; set; } = string.Empty;

    [Range(1, 120)]
    public int? ParticipantAge { get; set; }

    [MaxLength(100)]
    public string? OrganiserName { get; set; }

    [MaxLength(100)]
    public string? OrganisationName { get; set; }
}
