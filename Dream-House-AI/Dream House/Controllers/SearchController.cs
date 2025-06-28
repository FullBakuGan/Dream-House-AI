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
            var query = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .AsQueryable();

            if (min.HasValue)
                query = query.Where(a => a.cost >= min.Value);
            if (max.HasValue)
                query = query.Where(a => a.cost <= max.Value);

            var resultList = await query.ToListAsync();
            return View("Index", resultList);
        }

        public async Task<IActionResult> DistrictFilter(string district)
        {
            var query = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .AsQueryable();

            if (!string.IsNullOrEmpty(district))
                query = query.Where(a => a.district != null && EF.Functions.Like(a.district.district_name, $"%{district}%"));

            var resultList = await query.ToListAsync();
            return View("Index", resultList);
        }

        public async Task<IActionResult> RoomsFilter(int? count)
        {
            var query = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .AsQueryable();

            if (count.HasValue)
                query = query.Where(a => a.count_of_rooms != null && a.count_of_rooms == count.Value);

            var resultList = await query.ToListAsync();
            return View("Index", resultList);
        }

        public async Task<IActionResult> ResidentialComplex(string city_dis)
        {
            var query = _context.ads
                .Include(a => a.city)
                .Include(a => a.district)
                .Include(a => a.city_district)
                .Include(a => a.image_ads)
                .AsQueryable();

            if (!string.IsNullOrEmpty(city_dis))
                query = query.Where(a => a.city_district != null && EF.Functions.Like(a.city_district.city_district_name, $"%{city_dis}%"));

            var resultList = await query.ToListAsync();
            return View("Index", resultList);
        }
    }
}