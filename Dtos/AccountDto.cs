using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.DTOs;

public class AccountDto
{
    public int UserID { get; set; }

    [Required, MaxLength(55)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(55)]
    public string LastName { get; set; } = string.Empty;

    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string UserRole { get; set; } = string.Empty;
}