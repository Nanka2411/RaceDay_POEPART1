using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/entries")]
    [ApiController]
    [Authorize]
    public class EntriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public EntriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/entries
        [HttpPost]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> Create(
            [FromBody] CreateEntryDto dto)
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

            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.UserID == userId);

            if (participant == null)
            {
                return Forbid();
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == dto.EventID);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryID == dto.CategoryID);

            if (!categoryExists)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            var categoryLinkedToEvent = await _context.EventCategories
                .AnyAsync(ec =>
                    ec.EventID == dto.EventID &&
                    ec.CategoryID == dto.CategoryID);

            if (!categoryLinkedToEvent)
            {
                return BadRequest(new
                {
                    message = "This category is not available for the selected event."
                });
            }

            var alreadyEntered = await _context.Entries
                .AnyAsync(e =>
                    e.ParticipantID == participant.ParticipantID &&
                    e.EventID == dto.EventID);

            if (alreadyEntered)
            {
                return Conflict(new
                {
                    message = "You have already entered this event."
                });
            }

            int entryId = (await _context.Entries
                .MaxAsync(e => (int?)e.EntryID) ?? 0) + 1;

            var entry = new Entry
            {
                EntryID = entryId,
                ParticipantID = participant.ParticipantID,
                EventID = dto.EventID,
                CategoryID = dto.CategoryID,
                EntryDate = DateTime.Now,
                EntryStatus = "ENTERED"
            };

            _context.Entries.Add(entry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetMyEntries),
                new { },
                entry);
        }

        // GET: api/entries/mine
        [HttpGet("mine")]
        [Authorize(Roles = "Participant")]
        public async Task<IActionResult> GetMyEntries()
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

            var participant = await _context.Participants
                .FirstOrDefaultAsync(p => p.UserID == userId);

            if (participant == null)
            {
                return Forbid();
            }

            var entries = await _context.Entries
                .AsNoTracking()
                .Where(e => e.ParticipantID == participant.ParticipantID)
                .ToListAsync();

            return Ok(entries);
        }

        // GET: api/entries/event/1
        [HttpGet("event/{eventId:int}")]
        [Authorize(Roles = "Organiser")]
        public async Task<IActionResult> GetEntriesForEvent(int eventId)
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

            var organiser = await _context.Organisers
                .FirstOrDefaultAsync(o => o.UserID == userId);

            if (organiser == null)
            {
                return Forbid();
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == eventId);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            if (ev.OrganiserID != organiser.OrganiserID)
            {
                return Forbid();
            }

            var entries = await _context.Entries
                .AsNoTracking()
                .Where(e => e.EventID == eventId)
                .ToListAsync();

            return Ok(entries);
        }
    }
}