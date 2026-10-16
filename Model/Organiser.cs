
using System.Collections.Generic;

namespace RaceDayAPI.Models
{
    public class Organiser
    {
        public int OrganiserID { get; set; }

        public int UserID { get; set; }
        public Account? Account { get; set; }

        public string OrganiserName { get; set; } = string.Empty;

        public string OrganisationName { get; set; } = string.Empty;

        public ICollection<Event> Events { get; set; }
            = new List<Event>();
    }
}