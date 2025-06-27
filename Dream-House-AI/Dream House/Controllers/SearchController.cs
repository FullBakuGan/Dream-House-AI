using Dream_House.Data;
using hackaton_asp_project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Dream_House.Controllers
{
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ads = await _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .ToListAsync();

            return View(ads);
        }

        public async Task<IActionResult> CostFilter(int? min, int? max)
        {
            var ad = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .Where(a => (!min.HasValue || a.cost >= min.Value) && (!max.HasValue || a.cost <= max.Value));

            var resultList = await ad.ToListAsync();
            return View("Index", resultList);
        }

        public async Task<IActionResult> DistrictFilter(string district)
        {
            var ad = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .Where(a => district == null || (a.district != null && a.district.district_name.Contains(district)));

            var resultList = await ad.ToListAsync();
            return View("Index", resultList);
        }

        public async Task<IActionResult> RoomsFilter(int count)
        {
            var ad = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .Where(a => a.count_of_rooms == count);

            var resultList = await ad.ToListAsync();
            return View("Index", resultList);
        }

        public async Task<IActionResult> ResidentialComplex(string city_dis)
        {
            var ad = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .Where(a => city_dis == null || (a.city_district != null && a.city_district.city_district_name.Contains(city_dis)));

            var resultList = await ad.ToListAsync();
            return View("Index", resultList);
        }
    }
}