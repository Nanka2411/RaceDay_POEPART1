using System;

namespace RaceDayAPI.Models
{
    public class Entry
    {
        public int EntryID { get; set; }

        public int ParticipantID { get; set; }
        public Participant? Participant { get; set; }

        public int EventID { get; set; }
        public Event? Event { get; set; }

        public int CategoryID { get; set; }
        public EventCategory? EventCategory { get; set; }

        public DateTime EntryDate { get; set; } = DateTime.Now;

        public string EntryStatus { get; set; } = "ENTERED";

        public Result? Result { get; set; }
    }
}
