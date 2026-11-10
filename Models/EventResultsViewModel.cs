namespace RaceDayMVC.Models
{
    public class EventResultsViewModel
    {
        public int EventID { get; set; }

        public string EventName { get; set; } = "";

        public List<EntryDetailsViewModel> Entries { get; set; }
            = new List<EntryDetailsViewModel>();
    }
}
