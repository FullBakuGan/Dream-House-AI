using Microsoft.AspNetCore.Mvc;
using Dream_House.Services;
using System.Threading.Tasks;

namespace Dream_House.Controllers
{
    public class ChatBotController : Controller
    {
        private readonly ChatBotService _chatService;

        public ChatBotController(ChatBotService chatService)
        {
            _chatService = chatService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Message))
                    return BadRequest(new { error = "Сообщение не может быть пустым" });

                var response = await _chatService.GetResponseAsync(request.Message);

                if (string.IsNullOrEmpty(response))
                    return StatusCode(500, new { error = "Пустой ответ от сервера" });

                return Json(new { reply = response });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в контроллере: {ex}");
                return StatusCode(500, new
                {
                    error = "Ошибка при обработке запроса",
                    details = ex.Message
                });
            }
        }

        public class MessageRequest
        {
            public string Message { get; set; }
        }
    }
}