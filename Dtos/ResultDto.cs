using System.ComponentModel.DataAnnotations;

namespace RaceDayAPI.DTOs;

public class ResultDto
{
    [Required]
    public int EntryID { get; set; }

    [Required]
    [RegularExpression(
        @"^\d{1,2}:\d{2}:\d{2}$",
        ErrorMessage = "ResultTime must use HH:mm:ss format.")]
    public string ResultTime { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Position { get; set; }

    public string ResultStatus { get; set; } = "Completed";
}