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

//// ��������� ������ �����������
//var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
//    ?? builder.Configuration.GetConnectionString("DefaultConnection")
//    ?? throw new InvalidOperationException("DATABASE_URL or DefaultConnection not set");
//Console.WriteLine($"Original Connection String: {connectionString}"); // ��� �������

//// �������������� ������ ����������� �� ������� URI � ������ ����-��������
//if (connectionString.StartsWith("postgres://") || connectionString.StartsWith("postgresql://"))
//{
//    var uri = new Uri(connectionString);
//    var username = uri.UserInfo.Split(':')[0];
//    var password = uri.UserInfo.Split(':')[1];
//    var host = uri.Host;
//    var port = uri.Port != -1 ? uri.Port : 5432; // ���� ���� �� ������, ���������� 5432
//    var database = uri.AbsolutePath.TrimStart('/');
//    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
//}
//Console.WriteLine($"Parsed Connection String: {connectionString}"); // ��� �������

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


using Dream_House.Data;
using Dream_House.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Dream_House.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();

// Authorization, Controllers, Views
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();

// ��������� ������ �����������
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("DATABASE_URL or DefaultConnection not set");
Console.WriteLine($"Original Connection String: {connectionString}"); // ��� �������

// �������������� ������ ����������� �� URI
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
Console.WriteLine($"Parsed Connection String: {connectionString}"); // ��� �������

// ��������� ���� ������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// ��������� ��������������
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // ����� ������ ����
        options.LogoutPath = "/Account/Logout";
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// Cookie Policy
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

// SignalR + ���������������� ID ���������
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
            .WithOrigins("https://localhost:7224")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// DI
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ChatBotService>();
builder.Services.AddSingleton<IChat, ChatService>();

// ����������� ������������
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

// SignalR �������
app.MapHub<ChatHub>("/chathub");

// MVC ��������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();