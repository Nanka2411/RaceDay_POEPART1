namespace RaceDayMVC.Models
{
    public class EntryDetailsViewModel
    {
        public int EntryID { get; set; }

        public int EventID { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Phone { get; set; } = "";

        public int CategoryID { get; set; }

        public string CategoryName { get; set; } = "";

        public DateTime EntryDate { get; set; }

        public string EntryStatus { get; set; } = "";
    }
}