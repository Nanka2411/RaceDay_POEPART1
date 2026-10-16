using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/results")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public ResultsController(RaceDayDbContext context)
        {
            _context = context;
        }

        // POST: api/results
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ResultDto dto)
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

            var entry = await _context.Entries
                .FirstOrDefaultAsync(e => e.EntryID == dto.EntryID);

            if (entry == null)
            {
                return NotFound(new
                {
                    message = "Entry not found."
                });
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == entry.EventID);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "The event associated with this entry was not found."
                });
            }

            if (ev.OrganiserID != organiser.OrganiserID)
            {
                return Forbid();
            }

            var resultExists = await _context.Results
                .AnyAsync(r => r.EntryID == dto.EntryID);

            if (resultExists)
            {
                return Conflict(new
                {
                    message = "A result already exists for this entry."
                });
            }

            int resultId = (await _context.Results
                .MaxAsync(r => (int?)r.ResultID) ?? 0) + 1;

            var result = new Result
            {
                ResultID = resultId,
                EntryID = dto.EntryID,
                ResultTime = dto.ResultTime,
                Position = dto.Position,
                ResultStatus = dto.ResultStatus
            };

            _context.Results.Add(result);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEventResults),
                new { eventId = entry.EventID },
                result);
        }

        // PUT: api/results/1
        [Authorize(Roles = "Organiser")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] ResultDto dto)
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

            var result = await _context.Results
                .FirstOrDefaultAsync(r => r.ResultID == id);

            if (result == null)
            {
                return NotFound(new
                {
                    message = "Result not found."
                });
            }

            var entry = await _context.Entries
                .FirstOrDefaultAsync(e => e.EntryID == result.EntryID);

            if (entry == null)
            {
                return NotFound(new
                {
                    message = "The entry associated with this result was not found."
                });
            }

            var ev = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == entry.EventID);

            if (ev == null)
            {
                return NotFound(new
                {
                    message = "The event associated with this result was not found."
                });
            }

            if (ev.OrganiserID != organiser.OrganiserID)
            {
                return Forbid();
            }

            // If the entry is changed, ensure it exists and belongs
            // to an event managed by this organiser.
            var newEntry = await _context.Entries
                .FirstOrDefaultAsync(e => e.EntryID == dto.EntryID);

            if (newEntry == null)
            {
                return NotFound(new
                {
                    message = "The specified entry was not found."
                });
            }

            var newEvent = await _context.Events
                .FirstOrDefaultAsync(e => e.EventID == newEntry.EventID);

            if (newEvent == null ||
                newEvent.OrganiserID != organiser.OrganiserID)
            {
                return Forbid();
            }

            var duplicateResult = await _context.Results
                .AnyAsync(r =>
                    r.EntryID == dto.EntryID &&
                    r.ResultID != id);

            if (duplicateResult)
            {
                return Conflict(new
                {
                    message = "A result already exists for this entry."
                });
            }

            result.EntryID = dto.EntryID;
            result.ResultTime = dto.ResultTime;
            result.Position = dto.Position;
            result.ResultStatus = dto.ResultStatus;

            await _context.SaveChangesAsync();

            return Ok(result);
        }

        // GET: api/results/event/1
        [HttpGet("event/{eventId:int}")]
        public async Task<IActionResult> GetEventResults(int eventId)
        {
            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == eventId);

            if (!eventExists)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            var results = await _context.Results
                .AsNoTracking()
                .Join(
                    _context.Entries.AsNoTracking(),
                    result => result.EntryID,
                    entry => entry.EntryID,
                    (result, entry) => new
                    {
                        Result = result,
                        Entry = entry
                    })
                .Where(x => x.Entry.EventID == eventId)
                .Select(x => x.Result)
                .ToListAsync();

            return Ok(results);
        }

        // GET: api/results/mine
        [Authorize(Roles = "Participant")]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyResults()
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

            var results = await _context.Results
                .AsNoTracking()
                .Join(
                    _context.Entries.AsNoTracking(),
                    result => result.EntryID,
                    entry => entry.EntryID,
                    (result, entry) => new
                    {
                        Result = result,
                        Entry = entry
                    })
                .Where(x => x.Entry.ParticipantID == participant.ParticipantID)
                .Select(x => x.Result)
                .ToListAsync();

            return Ok(results);
        }
    }
}