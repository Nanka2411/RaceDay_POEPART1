using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTO;

namespace RaceDayAPI.Service;

public class EventService
{
    private readonly RaceDayDbContext _context;

    public EventService(RaceDayDbContext context)
    {
        _context = context;
    }

    public async Task<List<EventDto>> GetAllAsync()
    {
        return await _context.Events
            .AsNoTracking()
            .OrderBy(e => e.EventDate)
            .Select(e => new EventDto
            {
                EventID = e.EventID,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                EventLocation = e.EventLocation,
                RouteInformation = e.RouteInformation,
                Distance = e.Distance,
                EventType = e.EventType,
                OrganiserID = e.OrganiserID
            })
            .ToListAsync();
    }

    public async Task<EventDto?> GetByIdAsync(int id)
    {
        return await _context.Events
            .AsNoTracking()
            .Where(e => e.EventID == id)
            .Select(e => new EventDto
            {
                EventID = e.EventID,
                EventName = e.EventName,
                EventDescription = e.EventDescription,
                EventDate = e.EventDate,
                EventLocation = e.EventLocation,
                RouteInformation = e.RouteInformation,
                Distance = e.Distance,
                EventType = e.EventType,
                OrganiserID = e.OrganiserID
            })
            .SingleOrDefaultAsync();
    }
}