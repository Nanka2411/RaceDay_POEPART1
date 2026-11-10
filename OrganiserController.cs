


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDayMVC.Models;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiService _apiService;

        public OrganiserController(ApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("UserRole") == "Organiser";
        }

        public IActionResult Dashboard()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            return View();
        }

        // MANAGE EVENTS
        public async Task<IActionResult> Events()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                var events = await _apiService
                    .GetAsync<List<EventViewModel>>("api/events");

                return View(events ?? new List<EventViewModel>());
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "API ERROR: " + ex.Message;
                return View(new List<EventViewModel>());
            }
        }

        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _apiService.PostAsync<EventViewModel, EventViewModel>(
                    "api/events", model);

                TempData["SuccessMessage"] = "Event created successfully!";
                return RedirectToAction("Events");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "API ERROR: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditEvent(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                var ev = await _apiService
                    .GetAsync<EventViewModel>($"api/events/{id}");

                if (ev == null)
                    return NotFound();

                return View(ev);
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
                return RedirectToAction("Events");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEvent(int id, EventViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _apiService.PutAsync<EventViewModel, EventViewModel>(
                    $"api/events/{id}", model);

                TempData["SuccessMessage"] = "Event updated successfully!";
                return RedirectToAction("Events");
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError("", "API ERROR: " + ex.Message);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                await _apiService.DeleteAsync($"api/events/{id}");
                TempData["SuccessMessage"] = "Event deleted successfully!";
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
            }

            return RedirectToAction("Events");
        }

        // MANAGE CATEGORIES
        public async Task<IActionResult> Categories()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                var categories = await _apiService
                    .GetAsync<List<CategoryViewModel>>("api/categories");

                return View(categories ?? new List<CategoryViewModel>());
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "API ERROR: " + ex.Message;
                return View(new List<CategoryViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(CategoryViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                var categories = await _apiService
                    .GetAsync<List<CategoryViewModel>>("api/categories");

                return View("Categories", categories ?? new List<CategoryViewModel>());
            }

            try
            {
                await _apiService.PostAsync<CategoryViewModel, CategoryViewModel>(
                    "api/categories", model);

                TempData["SuccessMessage"] = "Category created successfully!";
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
            }

            return RedirectToAction("Categories");
        }

        // ASSIGN CATEGORIES TO EVENTS
        [HttpGet]
        public async Task<IActionResult> AssignCategory()
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                var categories = await _apiService
                    .GetAsync<List<CategoryViewModel>>("api/categories");

                var events = await _apiService
                    .GetAsync<List<EventViewModel>>("api/events");

                var model = new AssignCategoryViewModel
                {
                    Categories = categories ?? new List<CategoryViewModel>(),
                    Events = events ?? new List<EventViewModel>()
                };

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = "API ERROR: " + ex.Message;
                return View(new AssignCategoryViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignCategory(AssignCategoryViewModel model)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                await _apiService.PostNoContentAsync(
                    $"api/categories/{model.CategoryID}/events/{model.EventID}",
                    new { });

                TempData["SuccessMessage"] =
                    "Category successfully assigned to event.";
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
            }

            return RedirectToAction("AssignCategory");
        }

        // VIEW EVENT ENROLMENTS
        public async Task<IActionResult> Enrolments(int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                var ev = await _apiService
                    .GetAsync<EventViewModel>($"api/events/{eventId}");

                if (ev == null)
                    return NotFound();

                var enrolments = await _apiService
                    .GetAsync<List<EntryDetailsViewModel>>(
                        $"api/entries/event/{eventId}");

                var model = new EventEnrolmentsViewModel
                {
                    EventID = eventId,
                    EventName = ev.EventName,
                    Enrolments = enrolments ?? new List<EntryDetailsViewModel>()
                };

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
                return RedirectToAction("Events");
            }
        }

        // VIEW EVENT RESULTS
        public async Task<IActionResult> Results(int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                var ev = await _apiService
                    .GetAsync<EventViewModel>($"api/events/{eventId}");

                if (ev == null)
                    return NotFound();

                var entries = await _apiService
                    .GetAsync<List<EntryDetailsViewModel>>(
                        $"api/entries/event/{eventId}");

                var model = new EventResultsViewModel
                {
                    EventID = eventId,
                    EventName = ev.EventName,
                    Entries = entries ?? new List<EntryDetailsViewModel>()
                };

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
                return RedirectToAction("Events");
            }
        }

        // SAVE A PARTICIPANT'S RESULT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveResult(ResultViewModel model, int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] =
                    "Please enter a valid race time and finishing position.";

                return RedirectToAction("Results", new { eventId });
            }

            try
            {
                await _apiService.PostNoContentAsync("api/results", model);

                TempData["SuccessMessage"] =
                    "Participant result saved successfully!";
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
            }

            return RedirectToAction("Results", new { eventId });
        }

        // PUBLISH EVENT RESULTS
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PublishResults(int eventId)
        {
            if (!IsOrganiser())
                return RedirectToAction("Login", "Account");

            try
            {
                await _apiService.PutAsync<object, object>(
                    $"api/events/{eventId}/publish-results",
                    new { });

                TempData["SuccessMessage"] =
                    "Results published successfully!";
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = "API ERROR: " + ex.Message;
            }

            return RedirectToAction("Results", new { eventId });
        }
    }
}

