using System;
using System.Data;
using Microsoft.EntityFrameworkCore;
using RaceDayAPI.Data;
using RaceDayAPI.DTOs;
using RaceDayAPI.Models;

namespace RaceDayAPI.Services;

public class AccountService
{
    private readonly RaceDayDbContext _context;

    public AccountService(RaceDayDbContext context)
    {
        _context = context;
    }

    // REGISTER ACCOUNT
    public async Task<Account> RegisterAsync(CreateAccountDto dto)
    {
        string email = dto.Email.Trim().ToLowerInvariant();
        string role = dto.UserRole.Trim();

        if (role != "Participant" && role != "Organiser")
        {
            throw new ArgumentException(
                "Role must be Participant or Organiser.");
        }

        if (role == "Participant" &&
            (dto.ParticipantAge is null ||
             dto.ParticipantAge < 1 ||
             dto.ParticipantAge > 120))
        {
            throw new ArgumentException(
                "Participant age must be between 1 and 120.");
        }

        if (role == "Organiser" &&
            (string.IsNullOrWhiteSpace(dto.OrganiserName) ||
             string.IsNullOrWhiteSpace(dto.OrganisationName)))
        {
            throw new ArgumentException(
                "Organiser name and organisation name are required.");
        }

        if (await _context.Accounts.AnyAsync(a => a.Email == email))
        {
            throw new InvalidOperationException(
                "Email already exists.");
        }

        await using var transaction =
            await _context.Database.BeginTransactionAsync(
                IsolationLevel.Serializable);

        try
        {
            // Check again inside the transaction.
            if (await _context.Accounts.AnyAsync(a => a.Email == email))
            {
                throw new InvalidOperationException(
                    "Email already exists.");
            }

            // UserID is NOT IDENTITY in the existing SQL database.
            int highestUserId = await _context.Accounts
                .MaxAsync(a => (int?)a.UserID) ?? 0;

            var account = new Account
            {
                UserID = highestUserId + 1,
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Phone = dto.Phone.Trim(),
                UserRole = role,
                CreatedDate = DateTime.Now
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            if (role == "Participant")
            {
                var participant = new Participant
                {
                    ParticipantID =
                        await _context.Participants
                            .Select(p => (int?)p.ParticipantID)
                            .MaxAsync() is int maxParticipantId
                                ? maxParticipantId + 1
                                : 1,
                    UserID = account.UserID,
                    ParticipantAge = dto.ParticipantAge!.Value
                };

                _context.Participants.Add(participant);
            }
            else
            {
                var organiser = new Organiser
                {
                    OrganiserID =
                        await _context.Organisers
                            .Select(o => (int?)o.OrganiserID)
                            .MaxAsync() is int maxOrganiserId
                                ? maxOrganiserId + 1
                                : 1,
                    UserID = account.UserID,
                    OrganiserName = dto.OrganiserName!.Trim(),
                    OrganisationName = dto.OrganisationName!.Trim()
                };

                _context.Organisers.Add(organiser);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return account;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    // LOGIN
    public async Task<Account?> LoginAsync(
        string email,
        string password)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrEmpty(password))
        {
            return null;
        }

        string normalizedEmail =
            email.Trim().ToLowerInvariant();

        var account = await _context.Accounts
            .SingleOrDefaultAsync(a => a.Email == normalizedEmail);

        if (account is null)
        {
            return null;
        }

        try
        {
            return BCrypt.Net.BCrypt.Verify(
                password,
                account.PasswordHash)
                ? account
                : null;
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Existing sample hashes may not be BCrypt hashes.
            return null;
        }
    }

    // GET ACCOUNT BY ID
    public Task<Account?> GetByIdAsync(int userId)
    {
        return _context.Accounts
            .SingleOrDefaultAsync(a => a.UserID == userId);
    }

    // UPDATE PROFILE
    public async Task<Account?> UpdateProfileAsync(
        int userId,
        string firstName,
        string lastName,
        string phone)
    {
        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException(
                "First name, last name and phone are required.");
        }

        var account = await GetByIdAsync(userId);

        if (account is null)
        {
            return null;
        }

        account.FirstName = firstName.Trim();
        account.LastName = lastName.Trim();
        account.Phone = phone.Trim();

        await _context.SaveChangesAsync();

        return account;
    }
}