using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class BitrixTokenResponse
{
    public string AccessToken { get; set; }
    public int ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public string Scope { get; set; }
    public string Domain { get; set; }
}

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
        var redirectUri = _configuration["Bitrix24:RedirectUri"];
        var url = $"{portalUrl}/oauth/token/?grant_type=authorization_code&client_id={clientId}&client_secret={clientSecret}&code={code}&redirect_uri={redirectUri}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to get access token: {errorContent}");
        }

        var content = await response.Content.ReadAsStringAsync();
        var tokenData = JsonSerializer.Deserialize<BitrixTokenResponse>(content);
        if (string.IsNullOrEmpty(tokenData?.AccessToken))
        {
            throw new InvalidOperationException("Access token not found in response: " + content);
        }
        return tokenData.AccessToken;
    }
}