namespace RaceDayMVC.Models
{
    public class ParticipantEventDetailsViewModel
    {
        public int EventId { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime? EventDate { get; set; }
        public string Location { get; set; } = "";
        public string RouteInformation { get; set; } = "";
        public string Distance { get; set; } = "";
        public string EventType { get; set; } = "";

        public List<CategoryOptionViewModel> Categories { get; set; } = new();
    }

    public class CategoryOptionViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = "";
    }
}