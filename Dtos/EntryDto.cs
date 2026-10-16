using System;

namespace RaceDayAPI.DTOs
{
    public class EntryDto
    {
        public int EntryID { get; set; }

        public int ParticipantID { get; set; }

        public int EventID { get; set; }

        public int CategoryID { get; set; }

        public DateTime EntryDate { get; set; }

        public string EntryStatus { get; set; } = "ENTERED";
    }
}