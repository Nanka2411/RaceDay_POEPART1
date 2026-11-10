
namespace RaceDayMVC.Models
{
    public class ParticipantMyEventViewModel
    {
        public int EntryID { get; set; }
        public int EventID { get; set; }
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public string EventLocation { get; set; } = "";
        public int CategoryID { get; set; }
        public string CategoryName { get; set; } = "";
        public DateTime EntryDate { get; set; }
        public string EntryStatus { get; set; } = "";
        public bool ResultsPublished { get; set; }
        public string? ResultTime { get; set; }
        public int? Position { get; set; }
        public string? ResultStatus { get; set; }
    }
}
