using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RaceDayAPI.Data;

namespace RaceDayAPI.Tests
{
    public class CustomWebApplicationFactory
        : WebApplicationFactory<Program>
    {
        private readonly string _databaseName =
            Guid.NewGuid().ToString();

        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<
                    DbContextOptions<RaceDayDbContext>>();

                services.RemoveAll<RaceDayDbContext>();

                services.AddDbContext<RaceDayDbContext>(
                    options => options
                        .UseInMemoryDatabase(_databaseName)
                        .ConfigureWarnings(w =>
                            w.Ignore(
                                InMemoryEventId
                                    .TransactionIgnoredWarning)));
            });
        }
    }
}