
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models;

[Table("EventCategory")]
public class EventCategory
{
    public int EventID { get; set; }

    public int CategoryID { get; set; }

    public Event? Event { get; set; }

    public Category? Category { get; set; }

    public ICollection<Entry> Entries { get; set; } =
        new List<Entry>();
}
