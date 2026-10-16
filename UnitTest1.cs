using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace RaceDayAPI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task CreateEvent_WithoutLogin_ReturnsUnauthorized()
        {
            using var factory = new CustomWebApplicationFactory();

            using var client = factory.CreateClient(
                new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });

            var response = await client.PostAsync(
                "/api/events",
                new StringContent(
                    "{}",
                    System.Text.Encoding.UTF8,
                    "application/json"));

            Assert.Equal(
                HttpStatusCode.Unauthorized,
                response.StatusCode);
        }
    }
}