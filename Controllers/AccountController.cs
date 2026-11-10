

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using RaceDayMVC.Services;
using RaceDayMVC.Models;

namespace RaceDayMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // LOGIN PAGE
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // PROCESS LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _apiService
                    .PostAsync<LoginViewModel, ApiLoginResponse>(
                        "api/auth/login", model);

                if (result == null)
                {
                    ModelState.AddModelError("", "Login failed.");
                    return View(model);
                }

                HttpContext.Session.SetString(
                    "UserID", result.UserID.ToString());

                HttpContext.Session.SetString(
                    "Email", result.Email);

                HttpContext.Session.SetString(
                    "UserRole", result.UserRole);

                if (result.UserRole == "Organiser")
                {
                    return RedirectToAction(
                        "Dashboard", "Organiser");
                }

                return RedirectToAction(
                    "Events", "Participant");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "", "Invalid email or password. Please try again.");

                return View(model);
            }
        }

        // REGISTRATION PAGE
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // PROCESS REGISTRATION
        [HttpPost]
        public async Task<IActionResult> Register(
            RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _apiService.PostNoContentAsync(
                    "api/auth/register", model);

                TempData["SuccessMessage"] =
                    "Account created successfully. Please log in.";

                return RedirectToAction("Login");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(
                    "",
                    "Registration failed. Please check your details or try another email address.");

                return View(model);
            }
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
