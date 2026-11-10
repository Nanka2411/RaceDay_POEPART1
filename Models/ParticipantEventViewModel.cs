
namespace RaceDayMVC.Models
{
    public class ParticipantEventViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? EventDate { get; set; }
        public string Location { get; set; } = "";

        public string CategoryName { get; set; } = "";
    }
}
