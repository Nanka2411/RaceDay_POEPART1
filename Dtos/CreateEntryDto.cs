using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.DTOs;

public class CreateEntryDto
{
    [Required]
    public int EventID { get; set; }

    [Required]
    public int CategoryID { get; set; }
}