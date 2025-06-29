using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class BitrixController : Controller
{
    private readonly Bitrix24Auth _bitrixAuth;
    private readonly Bitrix24Client _bitrixClient;
    private readonly IConfiguration _configuration;

    public BitrixController(Bitrix24Auth bitrixAuth, Bitrix24Client bitrixClient, IConfiguration configuration)
    {
        _bitrixAuth = bitrixAuth;
        _bitrixClient = bitrixClient;
        _configuration = configuration;
    }

    [HttpGet("/Bitrix/Index")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("/api/bitrix/authorize")]
    public IActionResult Authorize()
    {
        var clientId = _configuration["Bitrix24:ClientId"];
        var portalUrl = _configuration["Bitrix24:PortalUrl"];
        var redirectUri = _configuration["Bitrix24:RedirectUri"];
        var authUrl = $"{portalUrl}/oauth/authorize/?client_id={clientId}&response_type=code&redirect_uri={redirectUri}";
        return Redirect(authUrl);
    }

    [HttpGet("/api/bitrix/callback")]
    public async Task<IActionResult> Callback(string code)
    {
        var accessToken = await _bitrixAuth.GetAccessToken(code);
        HttpContext.Session.SetString("BitrixAccessToken", accessToken);
        return RedirectToAction("Index", "Bitrix");
    }

    [HttpPost("/api/bitrix/create-lead")]
    public async Task<IActionResult> CreateLead([FromBody] LeadRequest request)
    {
        var accessToken = HttpContext.Session.GetString("BitrixAccessToken");
        if (string.IsNullOrEmpty(accessToken))
        {
            return Unauthorized("Access token not found. Please authorize.");
        }

        var result = await _bitrixClient.CreateLead(request.Title, request.Name, request.Phone, accessToken);
        return Ok(result);
    }
}

public class LeadRequest
{
    public string Title { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
}