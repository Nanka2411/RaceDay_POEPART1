namespace RaceDayAPI.DTO;

public class EventDto
{
    public int EventID { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string? EventDescription { get; set; }
    public DateTime EventDate { get; set; }
    public string? EventLocation { get; set; }
    public string? RouteInformation { get; set; }
    public decimal Distance { get; set; }
    public string EventType { get; set; } = string.Empty;
    public int OrganiserID { get; set; }
}