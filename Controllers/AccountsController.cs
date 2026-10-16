using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RaceDayAPI.DTOs;
using RaceDayAPI.Services;

namespace RaceDayAPI.Controllers;

[ApiController]
[Route("api/accounts")]
[Authorize]
public class AccountsController : ControllerBase
{
    private readonly AccountService _service;

    public AccountsController(AccountService service)
    {
        _service = service;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        int userId = CurrentUserId();

        if (userId <= 0)
            return Unauthorized();

        var account = await _service.GetByIdAsync(userId);

        if (account is null)
            return NotFound();

        return Ok(new AccountDto
        {
            UserID = account.UserID,
            FirstName = account.FirstName,
            LastName = account.LastName,
            Phone = account.Phone,
            Email = account.Email,
            UserRole = account.UserRole
        });
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateMyProfile(AccountDto dto)
    {
        int userId = CurrentUserId();

        if (userId <= 0)
            return Unauthorized();

        try
        {
            var account = await _service.UpdateProfileAsync(
                userId, dto.FirstName, dto.LastName, dto.Phone);

            if (account is null)
                return NotFound();

            return Ok(new AccountDto
            {
                UserID = account.UserID,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Phone = account.Phone,
                Email = account.Email,
                UserRole = account.UserRole
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int CurrentUserId()
    {
        return int.TryParse(
            User.FindFirstValue(ClaimTypes.NameIdentifier),
            out int id) ? id : 0;
    }
}