using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class Bitrix24Auth
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public Bitrix24Auth(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> GetAccessToken(string code)
    {
        var clientId = _configuration["Bitrix24:ClientId"];
        var clientSecret = _configuration["Bitrix24:ClientSecret"];
        var portalUrl = _configuration["Bitrix24:PortalUrl"];
        var url = $"{portalUrl}/oauth/token/?grant_type=authorization_code&client_id={clientId}&client_secret={clientSecret}&code={code}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
        return tokenData["access_token"];
    }
}