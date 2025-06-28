using Dream_House.Models;
using Dream_House.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Threading.Tasks;
using Dream_House.Data;
using Microsoft.EntityFrameworkCore;

namespace Dream_House.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ApplicationDbContext _context;

        public AccountController(IAuthService authService, ApplicationDbContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Title = "Вход";
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            ViewBag.Title = "Вход";

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (success, firstName, lastName, roleId) = await _authService.AuthenticateUserAsync(model.Email, model.Password);

            if (success)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Name, $"{firstName} {lastName}"),
                    new Claim(ClaimTypes.Role, roleId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("Cookies", claimsPrincipal);

                return roleId switch
                {
                    1 => RedirectToAction("Index", "Profile"),
                    2 => RedirectToAction("Index", "Profile"),
                    3 => RedirectToAction("Index", "Developer"),
                    4 => RedirectToAction("Index", "Admin"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            ModelState.AddModelError(string.Empty, "Неверный email или пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult Welcome(string firstName, string lastName)
        {
            ViewBag.Title = "Добро пожаловать";
            ViewBag.FirstName = firstName;
            ViewBag.LastName = lastName;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Покупатель" },
                new SelectListItem { Value = "2", Text = "Риелтор" },
                new SelectListItem { Value = "3", Text = "Застройщик" },
                new SelectListItem { Value = "4", Text = "Администратор" }
            };
            ViewBag.Roles = roles;

            return View(new RegisterViewModel { RoleId = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Title = "Регистрация";

            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Покупатель" },
                new SelectListItem { Value = "2", Text = "Риелтор" },
                new SelectListItem { Value = "3", Text = "Застройщик" },
                new SelectListItem { Value = "4", Text = "Администратор" }
            };

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var result = await _authService.RegisterUserAsync(model);
                if (!result)
                {
                    ModelState.AddModelError("", "Ошибка регистрации. Возможно, email уже занят.");
                    return View(model);
                }

                // Автоматический вход после регистрации
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                var claims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()), // Добавляем Claim для id
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Name, $"{model.Name} {model.Surname}"),
                    new Claim(ClaimTypes.MobilePhone, model.PhoneNumber.ToString()),
                    new Claim(ClaimTypes.Role, model.RoleId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("Cookies", claimsPrincipal);

                return model.RoleId switch
                {
                    1 => RedirectToAction("Index", "Client"),
                    2 => RedirectToAction("Index", "Realtor"),
                    3 => RedirectToAction("Index", "Developer"),
                    4 => RedirectToAction("Index", "Admin"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            catch
            {
                ModelState.AddModelError("", "Произошла ошибка при регистрации");
                return View(model);
            }
        }
    }
}