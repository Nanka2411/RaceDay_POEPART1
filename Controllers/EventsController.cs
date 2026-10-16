using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EventsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.Events
                .AsNoTracking()
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/events/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.Events
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            return Ok(ev);
        }

        // POST: api/events
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.UserID == userId);

            if (organiser == null)
            {
                return Forbid();
            }

            int eventId = (await _context.Events
                .MaxAsync(e => (int?)e.EventID) ?? 0) + 1;

            var ev = new Event
            {
                EventID = eventId,
                EventName = dto.EventName.Trim(),
                EventDescription = dto.EventDescription,
                EventDate = dto.EventDate,
                EventLocation = dto.EventLocation.Trim(),
                RouteInformation = dto.RouteInformation,
                Distance = dto.Distance,
                EventType = dto.EventType,
                OrganiserID = organiser.OrganiserID
            };

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = ev.EventID },
                ev);
        }

        // PUT: api/events/1
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] CreateEventDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.UserID == userId);

            if (organiser == null ||
                ev.OrganiserID != organiser.OrganiserID)
            {
                return Forbid();
            }

            ev.EventName = dto.EventName.Trim();
            ev.EventDescription = dto.EventDescription;
            ev.EventDate = dto.EventDate;
            ev.EventLocation = dto.EventLocation.Trim();
            ev.RouteInformation = dto.RouteInformation;
            ev.Distance = dto.Distance;
            ev.EventType = dto.EventType;

            await _context.SaveChangesAsync();

            return Ok(ev);
        }

        // DELETE: api/events/1
        [Authorize(Roles = "Organiser")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userIdClaim = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid user identity."
                });
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == id);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.UserID == userId);

            if (organiser == null ||
                ev.OrganiserID != organiser.OrganiserID)
            {
                return Forbid();
            }

            var hasEntries = await _context.Entries
                .AnyAsync(e => e.EventID == id);

            if (hasEntries)
            {
                return Conflict(new
                {
                    message = "This event cannot be deleted because entries already exist."
                });
            }

            var hasCategories = await _context.EventCategories
                .AnyAsync(ec => ec.EventID == id);

            if (hasCategories)
            {
                return Conflict(new
                {
                    message = "Remove the event's category links before deleting this event."
                });
            }

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}