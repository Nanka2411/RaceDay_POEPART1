using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AccountService _service;

    public AuthController(AccountService service)
    {
        _service = service;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateAccountDto dto)
    {
        try
        {
            var account = await _service.RegisterAsync(dto);

            return Ok(new
            {
                account.UserID,
                account.FirstName,
                account.LastName,
                account.Email,
                account.UserRole
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var account = await _service.LoginAsync(
            dto.Email,
            dto.Password
        );

        if (account is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        var claims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                account.UserID.ToString()
            ),
            new(ClaimTypes.Name, account.Email),
            new(ClaimTypes.Role, account.UserRole)
        };

        var identity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity)
        );

        return Ok(new
        {
            message = "Login successful.",
            account.UserID,
            account.Email,
            account.UserRole
        });
    }

    // GET PROFILE
    [HttpGet("profile/{id:int}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        var account = await _service.GetByIdAsync(id);

        if (account == null)
        {
            return NotFound(new
            {
                message = "Account not found."
            });
        }

        return Ok(new
        {
            account.UserID,
            account.FirstName,
            account.LastName,
            account.Email,
            account.Phone,
            account.UserRole,
            account.CreatedDate
        });
    }

    // LOGOUT
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        return Ok(new
        {
            message = "Logged out successfully."
        });
    }
}