﻿using Microsoft.AspNetCore.Mvc;

namespace Dream_House.Controllers
{
    public class ClientController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
