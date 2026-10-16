
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models;

[Table("Category")]
public class Category
{
    [Key]
    public int CategoryID { get; set; }

    [Required, MaxLength(80)]
    public string CategoryName { get; set; } = string.Empty;

    public string? CategoryDescription { get; set; }

    public ICollection<EventCategory> EventCategories { get; set; } =
        new List<EventCategory>();
}
