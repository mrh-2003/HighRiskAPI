using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HighRiskAPI.ExternalApis
{
    public class WebScrapingAPI
    {
        private static HttpClient _httpClient;
        private static string _accessToken;
        private static readonly string BaseUrl = "http://127.0.0.1:8000";
        private static bool _initialized = false;

        private WebScrapingAPI() {  }

        public static async Task InitializeHttpClient()
        {
            if (_initialized)
            {
                return;
            }

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);

            string username = Environment.GetEnvironmentVariable("API_USERNAME");
            string password = Environment.GetEnvironmentVariable("API_PASSWORD");

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("API_USERNAME or API_PASSWORD environment variables are not set.");
            }

            var formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });

            using (var client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(BaseUrl + "/token", formData);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authResult = JsonSerializer.Deserialize<AuthResult>(responseContent);
                    _accessToken = authResult.access_token;
                    var author = new AuthenticationHeaderValue("Bearer", _accessToken);
                    _httpClient.DefaultRequestHeaders.Authorization = author;
                }
                else
                {
                    throw new HttpRequestException($"Error al autenticarse: {response.StatusCode}");
                }
            }

            _initialized = true;
        }

        public static async Task<string> SearchOfac(string name)
        {
            await InitializeHttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync($"search_ofac/{name}");
            return await ProcessResponse(response);
        }

        public static async Task<string> SearchOffshoreLeaks(string name)
        {
            await InitializeHttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync($"search_offshore_leaks/{name}");
            return await ProcessResponse(response);
        }

        public static async Task<string> SearchTheWorldBank(string name)
        {
            await InitializeHttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync($"search_the_world_bank/{name}");
            return await ProcessResponse(response);
        }

        private static async Task<string> ProcessResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Error al llamar a la API: {response.StatusCode}");
            }
        }
    }
}
