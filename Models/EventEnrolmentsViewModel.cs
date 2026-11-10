namespace RaceDayMVC.Models
{
    public class EventEnrolmentsViewModel
    {
        public int EventID { get; set; }

        public string EventName { get; set; } = "";

        public List<EntryDetailsViewModel> Enrolments { get; set; }
            = new List<EntryDetailsViewModel>();
    }
}