using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class Bitrix24Client
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public Bitrix24Client(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> CreateLead(string title, string name, string phone, string accessToken)
    {
        var portalUrl = _configuration["Bitrix24:PortalUrl"];
        var url = $"{portalUrl}/rest/crm.lead.add.json?auth={accessToken}";
        var payload = new
        {
            fields = new
            {
                TITLE = title,
                NAME = name,
                PHONE = new[] { new { VALUE = phone, VALUE_TYPE = "WORK" } }
            }
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}