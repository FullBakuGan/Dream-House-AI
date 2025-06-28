using Microsoft.AspNetCore.Mvc;
using Dream_House.Services;
using System.Threading.Tasks;

namespace Dream_House.Controllers
{
    public class OpenAIController : Controller
    {
        private readonly OpenAIService _openAIService;
        public OpenAIController(OpenAIService openAIService)
        {
            _openAIService = openAIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Message))
            {
                return BadRequest("Message is required");
            }

            var response = await _openAIService.GetResponseAsync(message.Message);
            return Json(new { reply = response });
        }
    }
    public class ChatMessage
    {
        public string Message { get; set; }
    }
}