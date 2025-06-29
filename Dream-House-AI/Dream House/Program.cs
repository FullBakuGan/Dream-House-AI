////using Dream_House.Data;
////using Dream_House.Services;
////using Microsoft.AspNetCore.Authentication.Cookies;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;


////var builder = WebApplication.CreateBuilder(args);

////builder.Services.AddAuthorization();
////builder.Services.AddControllersWithViews();

////builder.Services.AddDbContext<ApplicationDbContext>(options =>
////    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
////builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
////    .AddCookie(options =>
////    {
////        options.LoginPath = "/Home/Index";
////    });

////builder.Services.AddScoped<IAuthService, AuthService>();
////builder.Services.AddTransient<ChatBotService>();
////var app = builder.Build();
////app.UseStaticFiles();
////app.UseRouting();
////app.UseAuthentication(); 
////app.UseAuthorization();

////app.MapControllerRoute(
////    name: "default",
////    pattern: "{controller=Home}/{action=Index}/{id?}");

////app.Run();

//using Dream_House.Data;
//using Dream_House.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthorization();
//builder.Services.AddControllersWithViews();

//// Получение строки подключения
//var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
//    ?? builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? throw new InvalidOperationException("DATABASE_URL or DefaultConnection not set");
//Console.WriteLine($"Original Connection String: {connectionString}"); // Для отладки

//// Преобразование строки подключения из формата URI в формат ключ-значение
//if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
//{
//    var uri = new Uri(connectionString);
//    var username = uri.UserInfo.Split(':')[0];
//    var password = uri.UserInfo.Split(':')[1];
//    var host = uri.Host;
//    var port = uri.Port != -1 ? uri.Port : 5432; // Если порт не указан, используем 5432
//    var database = uri.AbsolutePath.TrimStart('/');
//    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
//}
//Console.WriteLine($"Parsed Connection String: {connectionString}"); // Для отладки

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", builder =>
//    {
//        builder.AllowAnyOrigin()
//               .AllowAnyMethod()
//               .AllowAnyHeader();
//    });
//});

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(connectionString));

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Home/Index";
//    });

//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddTransient<ChatBotService>();
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddUserSecrets<Program>();
//}
//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

//var app = builder.Build();

//app.UseCors("AllowAll");
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();


//using Dream_House.Data;
//using Dream_House.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Dream_House.Services.Interfaces;

//var builder = WebApplication.CreateBuilder(args);

//// Logging
//builder.Logging.AddConsole();

//// Authorization, Controllers, Views
//builder.Services.AddAuthorization();
//builder.Services.AddControllersWithViews();
//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//builder.Services.AddHttpClient();
//builder.Services.AddSingleton<Bitrix24Auth>();
//builder.Services.AddSingleton<Bitrix24Client>();
//// Получение строки подключения
//var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
//    ?? builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? throw new InvalidOperationException("DATABASE_URL or DefaultConnection not set");
//Console.WriteLine($"Original Connection String: {connectionString}"); // Для отладки

//// Преобразование строки подключения из URI
//if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
//{
//    var uri = new Uri(connectionString);
//    var username = uri.UserInfo.Split(':')[0];
//    var password = uri.UserInfo.Split(':')[1];
//    var host = uri.Host;
//    var port = uri.Port != -1 ? uri.Port : 5432;
//    var database = uri.AbsolutePath.TrimStart('/');
//    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
//}
//Console.WriteLine($"Parsed Connection String: {connectionString}"); // Для отладки

//// Настройка базы данных
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(connectionString));

//// Настройка аутентификации
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Account/Login"; // более точный путь
//        options.LogoutPath = "/Account/Logout";
//        options.Cookie.SameSite = SameSiteMode.None;
//        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    });

//// Cookie Policy
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.None;
//});

//// SignalR + пользовательский ID провайдер
//builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
//builder.Services.AddSignalR(hubOptions =>
//{
//    hubOptions.EnableDetailedErrors = true;
//    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
//});

//// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll", policy =>
//    {
//        policy
//            .WithOrigins("https://localhost:7224")
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials();
//    });
//});
//builder.Services.AddHttpContextAccessor();
//// DI
//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddTransient<ChatBotService>();
//builder.Services.AddSingleton<IChat, ChatService>();

//// Подключение конфигураций
//if (builder.Environment.IsDevelopment())
//{
//    builder.Configuration.AddUserSecrets<Program>();
//}
//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

//var app = builder.Build();
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}
//app.MapGet("/api/bitrix/callback", async (string code, Bitrix24Auth bitrixAuth, IHttpContextAccessor httpContextAccessor) =>
//{
//    var accessToken = await bitrixAuth.GetAccessToken(code);
//    httpContextAccessor.HttpContext.Session.SetString("BitrixAccessToken", accessToken);
//    return Results.Redirect("/Bitrix/Index"); // Используем Results.Redirect
//}).RequireCors("AllowAll");
//// Middleware
//app.UseHttpsRedirection();
//app.UseCors("AllowAll");
//app.UseStaticFiles();
//app.UseRouting();
//app.UseSession();
//app.UseCookiePolicy();
//app.UseAuthentication();
//app.UseAuthorization();

//// SignalR маршрут
//app.MapHub<ChatHub>("/chathub");

//// MVC маршруты
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();


using Dream_House.Data;
using Dream_House.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Dream_House.Services.Interfaces;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();

// Authorization, Controllers, Views
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<Bitrix24Auth>();
builder.Services.AddSingleton<Bitrix24Client>();
builder.Services.AddHttpContextAccessor();

// Redis для сессий
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder.Configuration["RedisConnectionString"] ?? "your-redis-connection-string"; // Настройте в Render
//    options.InstanceName = "DreamHouseAI";
//});

// Data Protection с Redis
//builder.Services.AddDataProtection()
//    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(builder.Configuration["RedisConnectionString"] ?? "your-redis-connection-string"), "DataProtection-Keys")
//    .SetApplicationName("DreamHouseAI");

// Получение строки подключения
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DATABASE_URL or DefaultConnection not set");
Console.WriteLine($"Original Connection String: {connectionString}");

// Преобразование строки подключения из URI
if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
{
    var uri = new Uri(connectionString);
    var username = uri.UserInfo.Split(':')[0];
    var password = uri.UserInfo.Split(':')[1];
    var host = uri.Host;
    var port = uri.Port != -1 ? uri.Port : 5432;
    var database = uri.AbsolutePath.TrimStart('/');
    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}
Console.WriteLine($"Parsed Connection String: {connectionString}");

// Настройка базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Настройка аутентификации
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// SignalR + пользовательский ID провайдер
builder.Services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.EnableDetailedErrors = true;
    hubOptions.ClientTimeoutInterval = TimeSpan.FromMinutes(2);
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .WithOrigins("https://localhost:7224", "https://dream-house-ai-1.onrender.com")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// DI
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ChatBotService>();
builder.Services.AddSingleton<IChat, ChatService>();

// Подключение конфигураций
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.MapGet("/api/bitrix/callback", async (string code, Bitrix24Auth bitrixAuth, IHttpContextAccessor httpContextAccessor, ILogger<Program> logger) =>
{
    logger.LogInformation("Received code: {Code}", code);
    try
    {
        if (httpContextAccessor.HttpContext.Session == null)
        {
            logger.LogError("Session is not available.");
            return Results.Problem("Session is not available.", statusCode: 500);
        }

        var accessToken = await bitrixAuth.GetAccessToken(code);
        logger.LogInformation("Access token: {AccessToken}", accessToken);
        httpContextAccessor.HttpContext.Session.SetString("BitrixAccessToken", accessToken);
        return Results.Redirect("/Bitrix/Index");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error processing Bitrix callback: {Message}", ex.Message);
        return Results.Problem("Error processing callback: " + ex.Message, statusCode: 500);
    }
}).RequireCors("AllowAll");

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

// SignalR маршрут
app.MapHub<ChatHub>("/chathub");

// MVC маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();