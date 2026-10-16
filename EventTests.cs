using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace RaceDayAPI.Tests
{
    public class EventTests
    {
        [Fact]
        public async Task CreateEvent_WithoutLogin_ReturnsUnauthorized()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.PostAsync(
                "/api/events",
                new StringContent(
                    "{}",
                    Encoding.UTF8,
                    "application/json"));

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }

        [Fact]
        public async Task GetEvents_WithoutLogin_ReturnsSuccess()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/events");

            Assert.Equal(
                HttpStatusCode.OK,
                response.StatusCode);
        }

        [Fact]
        public async Task CreateEvent_AsParticipant_ReturnsForbidden()
        {
            using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient(
                new Microsoft.AspNetCore.Mvc.Testing
                    .WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var account = new
            {
                FirstName = "Test",
                LastName = "Participant",
                Email = "role-test@test.com",
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

            var loginResponse = await client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    Email = account.Email,
                    Password = account.Password
                });

            Assert.Equal(
                HttpStatusCode.OK,
                loginResponse.StatusCode);

            var eventData = new
            {
                EventName = "Test Race",
                EventDescription = "Testing event access",
                EventDate = DateTime.UtcNow.AddDays(7),
                EventLocation = "Cape Town",
                RouteInformation = "Test route",
                Distance = 5.0,
                EventType = "Run"
            };

            var response = await client.PostAsJsonAsync(
                "/api/events",
                eventData);

            Assert.Equal(
                HttpStatusCode.Forbidden,
                response.StatusCode);
        }

        [Fact]
        public async Task GetEvent_NonExistingId_ReturnsNotFound()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/events/999");

            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }

        [Fact]
        public async Task DeleteEvent_WithoutLogin_ReturnsUnauthorized()
        {
            using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient(
                new Microsoft.AspNetCore.Mvc.Testing
                    .WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var response = await client.DeleteAsync("/api/events/999");

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }

        [Fact]
        public async Task GetEvent_InvalidId_ReturnsNotFound()
        {
            using var factory = new CustomWebApplicationFactory();
            using var client = factory.CreateClient();

            var response = await client.GetAsync("/api/events/0");

            Assert.Equal(
                HttpStatusCode.NotFound,
                response.StatusCode);
        }
    }
}