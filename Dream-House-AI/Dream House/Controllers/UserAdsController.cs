using Microsoft.AspNetCore.Mvc;
using hackaton_asp_project.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Dream_House.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace hackaton_asp_project.Controllers
{
    public class UserAdsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserAdsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserAds
        public async Task<IActionResult> Index()
        {
            var currentUserIdString = User.FindFirst("id")?.Value;
            int? currentUserId = string.IsNullOrEmpty(currentUserIdString) ? (int?)null : int.Parse(currentUserIdString);
            var ads = await _context.AdUserCreateds
                .Include(a => a.Type)
                .Include(a => a.City)
                .Include(a => a.CityDistrict)
                .Include(a => a.District)
                .Include(a => a.Microdistrict)
                .Include(a => a.Street)
                .Include(a => a.Users)
                .ToListAsync();

            ViewBag.CurrentUserId = currentUserId;
            return View(ads);
        }

        // GET: UserAds/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.TypeList = _context.type_objs.ToList();
            ViewBag.CityList = _context.cities.ToList();
            ViewBag.DistrictList = _context.districts.ToList();
            ViewBag.CityDistrictList = _context.city_districts.ToList();
            ViewBag.MicrodistrictList = _context.microdistricts.ToList();
            ViewBag.StreetList = _context.streets.ToList();
            return View(new AdCreateViewModel());
        }

        // POST: UserAds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(AdCreateViewModel model)
        {
            var currentUserIdString = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(currentUserIdString) || !int.TryParse(currentUserIdString, out int currentUserId))
            {
                ModelState.AddModelError("", "Вы должны быть авторизованы для создания объявления.");
                ViewBag.TypeList = _context.type_objs.ToList();
                ViewBag.CityList = _context.cities.ToList();
                ViewBag.DistrictList = _context.districts.ToList();
                ViewBag.CityDistrictList = _context.city_districts.ToList();
                ViewBag.MicrodistrictList = _context.microdistricts.ToList();
                ViewBag.StreetList = _context.streets.ToList();
                return View(model);
            }

            if (ModelState.IsValid)
            {
                var ad = new AdUserCreated
                {
                    TypeId = model.TypeId,
                    DistrictId = model.DistrictId,
                    CityId = model.CityId,
                    CityDistrictId = model.CityDistrictId,
                    MicrodistrictId = model.MicrodistrictId,
                    StreetId = model.StreetId,
                    AreaObj = model.AreaObj,
                    Cost = model.Cost,
                    Description = model.Description,
                    CountOfRooms = model.CountOfRooms,
                    Stage = model.Stage,
                    ImageUrls = model.ImageUrls,
                    UserId = currentUserId
                };

                _context.Add(ad);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TypeList = _context.type_objs.ToList();
            ViewBag.CityList = _context.cities.ToList();
            ViewBag.DistrictList = _context.districts.ToList();
            ViewBag.CityDistrictList = _context.city_districts.ToList();
            ViewBag.MicrodistrictList = _context.microdistricts.ToList();
            ViewBag.StreetList = _context.streets.ToList();
            return View(model);
        }
    }
}