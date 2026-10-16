using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models;

[Table("Account")]
public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int UserID { get; set; }

    [Required, MaxLength(55)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(55)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required, MaxLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string UserRole { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public Organiser? Organiser { get; set; }

    public Participant? Participant { get; set; }
}
