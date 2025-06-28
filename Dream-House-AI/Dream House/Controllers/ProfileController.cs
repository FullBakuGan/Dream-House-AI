using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dream_House.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            var user = User.Identity?.Name;
            var user_email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user_phone_number = User.Claims.FirstOrDefault(n => n.Type == ClaimTypes.MobilePhone)?.Value;
            var userData = new { User = user, Email = user_email };
            return View(userData);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
