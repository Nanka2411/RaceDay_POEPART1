using System.Net;
using System.Net.Http.Json;

namespace RaceDayAPI.Tests
{
    public class AccountTests
    {
        [Fact]
        public async Task Register_WithInvalidData_ReturnsBadRequest()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "/api/auth/register",
                new { });

            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }

        [Fact]
        public async Task Login_WithInvalidData_ReturnsBadRequest()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.PostAsJsonAsync(
                "/api/auth/login",
                new { });

            Assert.Equal(
                HttpStatusCode.BadRequest,
                response.StatusCode);
        }

        [Fact]
        public async Task Register_ValidParticipant_ReturnsOK()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var account = new
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "participant@test.com",
                Password = "TestPass123!",
                Phone = "0712345678",
                UserRole = "Participant",
                ParticipantAge = 21
            };

            var response = await client.PostAsJsonAsync(
                "/api/auth/register",
                account);

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task Login_WithValidCredentials_ReturnsOK()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var account = new
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "login@test.com",
                Password = "TestPass123!",
                Phone = "0712345678",
                UserRole = "Participant",
                ParticipantAge = 21
            };

            var registerResponse = await client.PostAsJsonAsync(
                "/api/auth/register",
                account);

            Assert.Equal(
                HttpStatusCode.OK,
                registerResponse.StatusCode);

            var login = new
            {
                Email = "login@test.com",
                Password = "TestPass123!"
            };

            var response = await client.PostAsJsonAsync(
                "/api/auth/login",
                login);

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task Login_WithIncorrectPassword_ReturnsUnauthorized()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var account = new
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "wrong-password@test.com",
                Password = "CorrectPass123!",
                Phone = "0712345678",
                UserRole = "Participant",
                ParticipantAge = 21
            };

            var registerResponse = await client.PostAsJsonAsync(
                "/api/auth/register",
                account);

            Assert.Equal(
                HttpStatusCode.OK,
                registerResponse.StatusCode);

            var response = await client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    Email = account.Email,
                    Password = "WrongPass123!"
                });

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }
    }
}