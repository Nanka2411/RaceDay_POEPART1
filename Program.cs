
var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

// Add session services
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Allow access to the current HTTP context
builder.Services.AddHttpContextAccessor();

// Connect to the RaceDay API
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];

builder.Services.AddHttpClient("RaceDayAPI", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl!);
});

// Register ApiService
builder.Services.AddScoped<RaceDayMVC.Services.ApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Enable session management
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


