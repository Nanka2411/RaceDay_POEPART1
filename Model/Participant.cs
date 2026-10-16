
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RaceDayAPI.Models;

[Table("Participant")]
public class Participant
{
    [Key]
    public int ParticipantID { get; set; }

    public int UserID { get; set; }

    [Range(1, 120)]
    public int ParticipantAge { get; set; }

    public Account? Account { get; set; }

    public ICollection<Entry> Entries { get; set; } =
        new List<Entry>();
}
