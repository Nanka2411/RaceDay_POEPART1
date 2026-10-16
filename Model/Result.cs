
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models;

[Table("Result")]
public class Result
{
    [Key]
    public int ResultID { get; set; }

    public int EntryID { get; set; }

    [Required, MaxLength(20)]
    public string ResultTime { get; set; } = string.Empty;

    public int Position { get; set; }

    [Required, MaxLength(20)]
    public string ResultStatus { get; set; } = "Completed";

    public Entry? Entry { get; set; }
}
