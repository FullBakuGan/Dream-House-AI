using Microsoft.AspNetCore.Mvc;

namespace Dream_House.Controllers
{
    public class AdvertisementsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
