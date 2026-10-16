using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.Models;

namespace RaceDayAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayDbContext _context;

        public CategoriesController(RaceDayDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _context.Categories
                .AsNoTracking()
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/categories/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CategoryID == id);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            return Ok(category);
        }

        // POST: api/categories
        [Authorize(Roles = "Organiser")]
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assign the next ID because the SQL table
            // does not generate IDs automatically.
            category.CategoryID =
                (await _context.Categories
                    .MaxAsync(c => (int?)c.CategoryID) ?? 0) + 1;

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = category.CategoryID },
                category);
        }

        // POST: api/categories/{categoryId}/events/{eventId}
        [Authorize(Roles = "Organiser")]
        [HttpPost("{categoryId:int}/events/{eventId:int}")]
        public async Task<IActionResult> AddCategoryToEvent(
            int categoryId,
            int eventId)
        {
            var category = await _context.Categories
                .FindAsync(categoryId);

            if (category == null)
            {
                return NotFound(new
                {
                    message = "Category not found."
                });
            }

            var eventExists = await _context.Events
                .AnyAsync(e => e.EventID == eventId);

            if (!eventExists)
            {
                return NotFound(new
                {
                    message = "Event not found."
                });
            }

            var alreadyLinked = await _context.EventCategories
                .AnyAsync(ec =>
                    ec.CategoryID == categoryId &&
                    ec.EventID == eventId);

            if (alreadyLinked)
            {
                return Conflict(new
                {
                    message = "This category is already linked to the event."
                });
            }

            var eventCategory = new EventCategory
            {
                EventID = eventId,
                CategoryID = categoryId
            };

            _context.EventCategories.Add(eventCategory);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Category successfully linked to event.",
                eventId,
                categoryId
            });
        }
    }
}