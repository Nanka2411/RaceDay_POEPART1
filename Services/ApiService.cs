
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;

namespace RaceDayMVC.Services
{
    public class ApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApiService(
            IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task<HttpResponseMessage> SendAsync(
            HttpMethod method,
            string endpoint,
            object? data = null)
        {
            var client = _httpClientFactory.CreateClient("RaceDayAPI");

            using var request = new HttpRequestMessage(method, endpoint);

            var session = _httpContextAccessor.HttpContext?.Session;
            var apiCookie = session?.GetString("ApiCookie");

            if (!string.IsNullOrEmpty(apiCookie))
            {
                request.Headers.TryAddWithoutValidation(
                    "Cookie", apiCookie);
            }

            if (data != null)
            {
                request.Content = JsonContent.Create(data);
            }

            var response = await client.SendAsync(request);

            if (response.Headers.TryGetValues(
                "Set-Cookie", out var cookies))
            {
                var authCookie = cookies.FirstOrDefault(
                    c => c.StartsWith(
                        "RaceDay.Auth=",
                        StringComparison.OrdinalIgnoreCase));

                if (authCookie != null)
                {
                    session?.SetString(
                        "ApiCookie",
                        authCookie.Split(';')[0]);
                }
            }

            return response;
        }

        private async Task CheckResponseAsync(
            HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException(
                    $"API request failed with status code {(int)response.StatusCode}. " +
                    $"Details: {errorBody}",
                    null,
                    response.StatusCode);
            }
        }

        // GET
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            using var response = await SendAsync(
                HttpMethod.Get, endpoint);

            await CheckResponseAsync(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        // POST with response
        public async Task<TResponse?> PostAsync<TRequest, TResponse>(
            string endpoint, TRequest data)
        {
            using var response = await SendAsync(
                HttpMethod.Post, endpoint, data);

            await CheckResponseAsync(response);

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        // PUT with response
        public async Task<TResponse?> PutAsync<TRequest, TResponse>(
            string endpoint, TRequest data)
        {
            using var response = await SendAsync(
                HttpMethod.Put, endpoint, data);

            await CheckResponseAsync(response);

            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        // DELETE
        public async Task DeleteAsync(string endpoint)
        {
            using var response = await SendAsync(
                HttpMethod.Delete, endpoint);

            await CheckResponseAsync(response);
        }

        // POST without response body
        public async Task PostNoContentAsync<TRequest>(
            string endpoint, TRequest data)
        {
            using var response = await SendAsync(
                HttpMethod.Post, endpoint, data);

            await CheckResponseAsync(response);
        }
    }
}
