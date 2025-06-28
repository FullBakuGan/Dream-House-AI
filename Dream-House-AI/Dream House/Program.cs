//using Dream_House.Data;
//using Dream_House.Services;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;


//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthorization();
//builder.Services.AddControllersWithViews();

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Home/Index";
//    });

//builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddTransient<ChatBotService>();
//var app = builder.Build();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthentication(); 
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();

using Dream_House.Data;
using Dream_House.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

// Получение строки подключения
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DATABASE_URL or DefaultConnection not set");
Console.WriteLine($"Original Connection String: {connectionString}"); // Для отладки

// Преобразование строки подключения из формата URI в формат ключ-значение
if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
{
    var uri = new Uri(connectionString);
    var username = uri.UserInfo.Split(':')[0];
    var password = uri.UserInfo.Split(':')[1];
    var host = uri.Host;
    var port = uri.Port != -1 ? uri.Port : 5432; // Если порт не указан, используем 5432
    var database = uri.AbsolutePath.TrimStart('/');
    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
}
Console.WriteLine($"Parsed Connection String: {connectionString}"); // Для отладки

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index";
    });

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ChatBotService>();

var app = builder.Build();

// Применение миграций при старте
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//    try
//    {
//        dbContext.Database.Migrate();
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Migration error: {ex.Message}");
//        throw;
//    }
//}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();