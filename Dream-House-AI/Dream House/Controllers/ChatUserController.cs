using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class ChatUserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}