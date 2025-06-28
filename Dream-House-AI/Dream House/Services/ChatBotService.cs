//using System.Net.Http.Headers;
//using System.Text;
//using Newtonsoft.Json;

//namespace Dream_House.Services
//{
//    public class ChatBotService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _apiKey;

//        public ChatBotService(IConfiguration configuration)
//        {
//            _httpClient = new HttpClient();
//            _apiKey = configuration["DeepSeek:ApiKey"];
//            _httpClient.DefaultRequestHeaders.Authorization =
//                new AuthenticationHeaderValue("Bearer", _apiKey);
//            _httpClient.Timeout = TimeSpan.FromSeconds(30);
//        }

//        public async Task<string> GetResponseAsync(string prompt)
//        {
//            try
//            {
//                var request = new
//                {
//                    model = "deepseek-chat",
//                    messages = new[] { new { role = "user", content = prompt } },
//                    temperature = 0.7,
//                    max_tokens = 2000
//                };

//                var content = new StringContent(
//                    JsonConvert.SerializeObject(request),
//                    Encoding.UTF8,
//                    "application/json");

//                var response = await _httpClient.PostAsync(
//                    "https://api.deepseek.com/v1/chat/completions",
//                    content);

//                var responseContent = await response.Content.ReadAsStringAsync();

//                if (!response.IsSuccessStatusCode)
//                {
//                    throw new Exception($"API Error: {response.StatusCode}\n{responseContent}");
//                }

//                dynamic json = JsonConvert.DeserializeObject(responseContent);
//                return json.choices[0].message.content.ToString();
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"ChatBotService error: {ex}");
//                return "Извините, произошла ошибка. Пожалуйста, попробуйте позже.";
//            }
//        }
//    }
//}

using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Dream_House.Services
{
    public class ChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _folderId;

        public ChatBotService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["YandexGPT:ApiKey"];
            _folderId = configuration["YandexGPT:FolderId"];
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Api-Key", _apiKey);
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> GetResponseAsync(string prompt)
        {
            try
            {
                var request = new
                {
                    modelUri = $"gpt://{_folderId}/yandexgpt-lite/latest",
                    completionOptions = new
                    {
                        stream = false,
                        temperature = 0.6,
                        maxTokens = 2000
                    },
                    messages = new[]
                    {
                        new
                        {
                            role = "user",
                            text = prompt
                        }
                    }
                };

                var content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(
                    "https://llm.api.cloud.yandex.net/foundationModels/v1/completion",
                    content);

                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Yandex API Error: {response.StatusCode}\n{responseContent}");
                }

                dynamic json = JsonConvert.DeserializeObject(responseContent);
                return json.result.alternatives[0].message.text.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ChatBotService error: {ex}");
                return "Извините, произошла ошибка. Пожалуйста, попробуйте позже.";
            }
        }
    }
}