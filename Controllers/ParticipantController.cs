
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using RaceDayMVC.Models;
using RaceDayMVC.Services;

namespace RaceDayMVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiService _apiService;

        public ParticipantController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Display all available events
        public async Task<IActionResult> Events()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            try
            {
                var apiEvents = await _apiService
                    .GetAsync<List<JsonElement>>("api/events");

                var events = new List<ParticipantEventViewModel>();

                if (apiEvents != null)
                {
                    foreach (var item in apiEvents)
                    {
                        events.Add(new ParticipantEventViewModel
                        {
                            EventId = GetInt(item, "EventId", "EventID", "Id"),
                            Title = GetString(item, "EventName", "Name", "Title"),
                            Description = GetString(item, "EventDescription", "Description"),
                            Location = GetString(item, "EventLocation", "Location", "Venue"),
                            EventDate = GetDate(item, "EventDate", "Date", "StartDate")
                        });
                    }
                }

                return View(events);
            }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View(new List<ParticipantEventViewModel>());
            }
        }

        // Display details and categories for one event
        public async Task<IActionResult> Details(int id)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            try
            {
                var apiEvent = await _apiService
                    .GetAsync<JsonElement>($"api/events/{id}");

                if (apiEvent.ValueKind == JsonValueKind.Undefined ||
                    apiEvent.ValueKind == JsonValueKind.Null)
                    return NotFound();

                var apiCategories = await _apiService
                    .GetAsync<List<JsonElement>>($"api/categories/event/{id}");

                var model = new ParticipantEventDetailsViewModel
                {
                    EventId = GetInt(apiEvent, "EventId", "EventID", "Id"),
                    Title = GetString(apiEvent, "EventName", "Name", "Title"),
                    Description = GetString(apiEvent, "EventDescription", "Description"),
                    Location = GetString(apiEvent, "EventLocation", "Location", "Venue"),
                    EventDate = GetDate(apiEvent, "EventDate", "Date", "StartDate"),
                    RouteInformation = GetString(apiEvent, "RouteInformation"),
                    Distance = GetString(apiEvent, "Distance"),
                    EventType = GetString(apiEvent, "EventType")
                };

                if (apiCategories != null)
                {
                    foreach (var category in apiCategories)
                    {
                        model.Categories.Add(new CategoryOptionViewModel
                        {
                            CategoryId = GetInt(category, "CategoryID", "CategoryId", "Id"),
                            CategoryName = GetString(category, "CategoryName", "Name")
                        });
                    }
                }

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Events));
            }
        }

        // Enrol participant in an event
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enrol(int eventId, int categoryId)
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            if (categoryId <= 0)
            {
                TempData["ErrorMessage"] = "Please select a category.";
                return RedirectToAction(nameof(Details), new { id = eventId });
            }

            try
            {
                await _apiService.PostNoContentAsync(
                    "api/entries",
                    new
                    {
                        EventID = eventId,
                        CategoryID = categoryId
                    });

                TempData["SuccessMessage"] =
                    "You have successfully enrolled in this event.";
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    TempData["ErrorMessage"] =
                        "You have already enrolled in this event.";
                }
                else if (ex.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    TempData["ErrorMessage"] =
                        "The selected category is not available for this event. " + ex.Message;
                }
                else
                {
                    TempData["ErrorMessage"] = ex.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] =
                    "An unexpected error occurred: " + ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id = eventId });
        }

        // Display events entered by the logged-in participant
        public async Task<IActionResult> MyEvents()
        {
            if (HttpContext.Session.GetString("UserRole") != "Participant")
                return RedirectToAction("Login", "Account");

            try
            {
                var apiEntries = await _apiService
                    .GetAsync<List<JsonElement>>("api/entries/mine");

                var model = new List<ParticipantMyEventViewModel>();

                if (apiEntries != null)
                {
                    foreach (var item in apiEntries)
                    {
                        model.Add(new ParticipantMyEventViewModel
                        {
                            EntryID = GetInt(item, "EntryID"),
                            EventID = GetInt(item, "EventID"),
                            EventName = GetString(item, "EventName"),
                            EventDate = GetDate(item, "EventDate") ?? DateTime.MinValue,
                            EventLocation = GetString(item, "EventLocation"),
                            CategoryID = GetInt(item, "CategoryID"),
                            CategoryName = GetString(item, "CategoryName"),
                            EntryDate = GetDate(item, "EntryDate") ?? DateTime.MinValue,
                            EntryStatus = GetString(item, "EntryStatus"),
                            ResultsPublished = GetBool(item, "ResultsPublished"),
                            ResultTime = GetNullableString(item, "ResultTime"),
                            Position = GetNullableInt(item, "Position"),
                            ResultStatus = GetNullableString(item, "ResultStatus")
                        });
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Unable to load your events: " + ex.Message;
                return View(new List<ParticipantMyEventViewModel>());
            }
        }

        // JSON helper methods
        private static JsonElement FindProperty(
            JsonElement item, params string[] names)
        {
            if (item.ValueKind != JsonValueKind.Object)
                return default;

            foreach (var property in item.EnumerateObject())
            {
                if (names.Any(name =>
                    string.Equals(property.Name, name,
                        StringComparison.OrdinalIgnoreCase)))
                {
                    return property.Value;
                }
            }

            return default;
        }

        private static string GetString(
            JsonElement item, params string[] names)
        {
            var value = FindProperty(item, names);

            if (value.ValueKind == JsonValueKind.String)
                return value.GetString() ?? "";

            if (value.ValueKind == JsonValueKind.Number)
                return value.ToString();

            return "";
        }

        private static string? GetNullableString(
            JsonElement item, params string[] names)
        {
            var value = FindProperty(item, names);

            if (value.ValueKind == JsonValueKind.Null ||
                value.ValueKind == JsonValueKind.Undefined)
                return null;

            return value.ToString();
        }

        private static int GetInt(
            JsonElement item, params string[] names)
        {
            var value = FindProperty(item, names);

            if (value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt32(out int result))
                return result;

            if (value.ValueKind == JsonValueKind.String &&
                int.TryParse(value.GetString(), out result))
                return result;

            return 0;
        }

        private static int? GetNullableInt(
            JsonElement item, params string[] names)
        {
            var value = FindProperty(item, names);

            if (value.ValueKind == JsonValueKind.Number &&
                value.TryGetInt32(out int result))
                return result;

            if (value.ValueKind == JsonValueKind.String &&
                int.TryParse(value.GetString(), out result))
                return result;

            return null;
        }

        private static bool GetBool(
            JsonElement item, params string[] names)
        {
            var value = FindProperty(item, names);

            return value.ValueKind == JsonValueKind.True;
        }

        private static DateTime? GetDate(
            JsonElement item, params string[] names)
        {
            var value = FindProperty(item, names);

            if (value.ValueKind == JsonValueKind.String &&
                DateTime.TryParse(value.GetString(), out DateTime date))
                return date;

            return null;
        }
    }
}

